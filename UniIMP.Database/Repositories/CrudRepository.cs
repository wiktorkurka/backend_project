using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UniIMP.DataAccess.Repositories
{
    public class CrudRepository<T> : ICrudRepository<T>, IDisposable where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        internal DbSet<T> entitySet;

        public CrudRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            entitySet = _dbContext.Set<T>();
        }

        public IEnumerable<T> GetAll() =>
            entitySet.ToList();

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await entitySet.ToListAsync();

        public T? Get(int Id) =>
            entitySet.Find(Id);

        public async Task<T?> GetAsync(int Id) =>
            await entitySet.FindAsync(Id);

        public void Create(T entity) =>
            entitySet.Add(entity);

        public void Create(IEnumerable<T> entities) =>
            entitySet.AddRange(entities);

        public async Task CreateAsync(T entity) =>
            await entitySet.AddAsync(entity);

        public async Task CreateAsync(IEnumerable<T> entity) =>
            await entitySet.AddRangeAsync(entity);

        public void Remove(int Id)
        {
            var asset = entitySet.Find(Id);
            if (asset != null)
                entitySet.Remove(asset);
        }

        public void Remove(IEnumerable<int> IDs)
        {
            foreach (var id in IDs)
            {
                var asset = entitySet.Find(id);
                if (asset != null)
                    entitySet.Remove(asset);
            }
        }

        public void Update(T entity) =>
            _dbContext.Entry(entity).State = EntityState.Modified;

        public void Save() =>
            _dbContext.SaveChanges();

        public async Task SaveAsync() =>
            await _dbContext.SaveChangesAsync();

        public IQueryable<T> GetQueryable() =>
            entitySet.AsQueryable();

        // Loading Related Entites
        public void LoadRelated(T entity)
        {
            var entityCollections = _dbContext.Entry(entity).Collections;
            foreach (var collection in entityCollections)
                collection.Load();
        }

        public void LoadRelated(T entity, string collectionName) =>
            _dbContext.Entry(entity).Collection(collectionName).Load();

        public void LoadRelated<TProperty>(
            T entity,
            Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
            where TProperty : class =>
                _dbContext.Entry(entity).Collection(propertyExpression).Load();

        public async Task LoadRelatedAsync(T entity)
        {
            var entityCollections = _dbContext.Entry(entity).Collections;
            foreach (var collection in entityCollections)
                await collection.LoadAsync();
        }

        public async Task LoadRelatedAsync(T entity, string collectionName) =>
            await _dbContext.Entry(entity).Collection(collectionName).LoadAsync();

        public async Task LoadRelatedAsync<TProperty>(
            T entity,
            Expression<Func<T, IEnumerable<TProperty>>> propertyExpression)
            where TProperty : class =>
                await _dbContext.Entry(entity).Collection(propertyExpression).LoadAsync();

        // Dispose logic
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
                _dbContext.Dispose();

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}