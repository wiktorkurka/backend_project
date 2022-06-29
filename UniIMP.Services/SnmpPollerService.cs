using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;
using UniIMP.Utility.JsonConverters;

namespace UniIMP.Services
{
    public class SnmpPollerService
    {
        private readonly ICrudRepository<SnmpAgent> _agentRepository;
        private readonly ICrudRepository<Asset> _assetRepository;

        public SnmpPollerService(
            ICrudRepository<SnmpAgent> agentRepository,
            ICrudRepository<Asset> assetRepositpry)
        {
            _agentRepository = agentRepository;
            _assetRepository = assetRepositpry;
        }

        public object? ConvertSnmpData(ISnmpData snmpData)
        {
            try
            {
                switch (snmpData.TypeCode)
                {
                    case SnmpType.Integer32:
                        return BitConverter.ToInt32(snmpData.ToBytes());

                    case SnmpType.Unsigned32:
                        return BitConverter.ToUInt32(snmpData.ToBytes());

                    case SnmpType.OctetString or SnmpType.ObjectIdentifier:
                        var s = snmpData.ToString().Replace("\0", string.Empty);
                        //var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                        //if (regexItem.IsMatch(s))
                        //    return s;
                        //else return null;
                        return s;

                    case SnmpType.Null:
                        return null;

                    default:
                        return snmpData.ToString();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Poll(int id, string OID = "1.3.6.1.2.1")
        {
            var agent = _agentRepository.Get(id);

            if (agent == null)
                return;

            List<Variable> pollResult = new List<Variable>();

            Messenger.BulkWalk(
                VersionCode.V2,
                new IPEndPoint(agent.IpAddress, 161),
                new OctetString(agent.Community),
                OctetString.Empty,
                new ObjectIdentifier(OID),
                pollResult,
                10, 6000, WalkMode.Default, null, null);

            Console.WriteLine();
        }

        public async Task PollAsync(int id, string OID = ".")
        {
            var agent = await _agentRepository.GetAsync(id);

            if (agent == null)
                return;

            await _agentRepository.LoadRelatedAsync(agent);

            List<Variable> pollResult = new List<Variable>();

            var pollTask = Messenger.BulkWalkAsync(
                VersionCode.V2,
                new IPEndPoint(agent.IpAddress, 161),
                new OctetString(agent.Community),
                OctetString.Empty,
                new ObjectIdentifier(OID),
                pollResult,
                1, WalkMode.Default, null, null);

            if (await Task.WhenAny(pollTask, Task.Delay(10000)) == pollTask) // When Poll is successful
            {
                agent.State = AgentState.Up;
                agent.LastSeen = DateTimeOffset.UtcNow;

                var asset = agent.Asset;

                if (asset != null)
                {

                    Dictionary<string,object> result = new Dictionary<string,object>();
                    // Convert SNMP Data to readable format
                    foreach (var variable in pollResult)
                    {
                        var key = variable.Id.ToString();
                        var value = ConvertSnmpData(variable.Data);

                        if (value != null) 
                            result.Add(key, value);
                    }
                    // Translate OIDs to MIBs

                    // Add SNMP Poll results to Asset
                    var propertyName = "SnmpResult";
                    var properties = asset.Properties;

                    if (properties.ContainsKey(propertyName))
                        properties.Remove(propertyName);

                    properties.Add(propertyName, JToken.FromObject(result)) ;

                    _assetRepository.Update(asset);
                    await _assetRepository.SaveAsync();
                }
            }
            else
                agent.State = AgentState.Down;

            _agentRepository.Update(agent);
            await _agentRepository.SaveAsync();
        }
    }
}