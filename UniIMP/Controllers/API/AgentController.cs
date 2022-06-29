using Microsoft.AspNetCore.Mvc;
using UniIMP.DataAccess;
using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;
using UniIMP.Services;

namespace UniIMP.Controllers.API
{
    public class AgentController : CrudController<SnmpAgent>
    {
        private readonly ICrudRepository<SnmpAgent> _agentRepository;
        private readonly SnmpPollerService _snmpPollerService;

        public AgentController(
            ApplicationDbContext dbContext,
            ICrudRepository<SnmpAgent> agentRepository,
            SnmpPollerService snmpPollerService)
            : base(agentRepository)
        {
            _agentRepository = agentRepository;
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
    }
}