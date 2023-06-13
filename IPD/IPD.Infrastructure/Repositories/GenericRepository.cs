using IPD.Application.Interfaces.Repositories;
using IPD.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IPD.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate)
        {
            return this._dbContext.Set<T>()
            .Where(predicate);
        }

        public async Task<ICollection<T>> FindAllByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate,
       params Expression<Func<T, object>>[] includes)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            return query.FirstOrDefault(predicate);
        }

        public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
        {
            return await includes
              .Aggregate(_dbContext.Set<T>().AsQueryable(),
                  (entity, property) => entity.Include(property)).Where(predicate)
              .ToListAsync();
        }
    }
}
