using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UniIMP.DataAccess.Entities;

namespace UniIMP.DataAccess.Repositories
{
    public interface ICrudRepository<T> : IDisposable where T : class
    {
        public IEnumerable<T> GetAll();
        public Task<IEnumerable<T>> GetAllAsync();
        public T? Get(int Id);
        public Task<T?> GetAsync(int Id);
        public void Create(T entity);
        public void Create(IEnumerable<T> entities);
        public Task CreateAsync(T entity);
        public Task CreateAsync(IEnumerable<T> entity);
        public void Remove(int Id);
        public void Remove(IEnumerable<int> IDs);
        public void Update(T entity);
        public void Save();
        public Task SaveAsync();
        public IQueryable<T> GetQueryable();


    }
}
