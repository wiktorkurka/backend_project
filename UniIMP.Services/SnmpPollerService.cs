using UniIMP.DataAccess.Entities;
using UniIMP.DataAccess.Repositories;

namespace UniIMP.Services
{
    public class SnmpPollerService
    {
        private readonly ICrudRepository<SnmpAgent> _repository;
        public SnmpPollerService(ICrudRepository<SnmpAgent> repository)
        {
            _repository = repository;
        }

    }
}