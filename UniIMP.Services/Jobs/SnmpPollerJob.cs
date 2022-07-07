using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Services.Jobs
{
    public class SnmpPollerJob : IJob
    {

        private readonly ICrudRepository<SnmpAgent> _agentRepository;
        private readonly SnmpPollerService _pollerService;

        public SnmpPollerJob(
            ICrudRepository<SnmpAgent> agentRepository,
            SnmpPollerService pollerService)
        {
            _agentRepository = agentRepository;
            _pollerService = pollerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var agents = _agentRepository
                                    .GetQueryable()
                                    .Where(agent => 
                                    (((DateTimeOffset.UtcNow - agent.LastUpdated).TotalMinutes >= 5) &&
                                        (agent.State == AgentState.Up)) || 
                                    (((DateTimeOffset.UtcNow - agent.LastUpdated).TotalMinutes >= 30) &&
                                        (agent.State == AgentState.Down)))
                                    .AsEnumerable().ToList();

            foreach (var agent in agents) {
                Console.WriteLine($"Polling @ {agent.IpAddress.ToString()}");
                await _pollerService.PollAsync(agent.Id);
            }
        }
    }
}
