using Microsoft.AspNetCore.Mvc;
using System.Net;
using UniIMP.DataAccess;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;
using UniIMP.Models;
using UniIMP.Services;

namespace UniIMP.Controllers.API
{
    public class AgentsController : CrudController<SnmpAgent>
    {
        private readonly ICrudRepository<SnmpAgent> _agentRepository;
        private readonly ICrudRepository<Asset> _assetRepository;
        private readonly SnmpPollerService _snmpPollerService;

        public AgentsController(
            ApplicationDbContext dbContext,
            ICrudRepository<SnmpAgent> agentRepository,
            ICrudRepository<Asset> assetRepository,
            SnmpPollerService snmpPollerService)
            : base(agentRepository)
        {
            _agentRepository = agentRepository;
            _assetRepository = assetRepository;
            _snmpPollerService = snmpPollerService;
        }

        [HttpGet("{id?}/poll")]
        public async Task<IActionResult> AgentPoll(
            int id,
            [FromQuery] string? oid)
        {
            if (oid != null)
                await _snmpPollerService.PollAsync(id, oid);
            else
                await _snmpPollerService.PollAsync(id);

            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] SnmpAgentIntermediateModel model) {
            try {
            SnmpAgent agent = new SnmpAgent() {
                Asset = await _assetRepository.GetAsync(model.AssetId),
                IpAddress = IPAddress.Parse(model.IpAddress),
                Community = model.Community
            };
            
            await _agentRepository.CreateAsync(agent);
            await _agentRepository.SaveAsync();

            return Ok();
            } catch (FormatException) {
                return BadRequest(model);
            }
        }
    }
}