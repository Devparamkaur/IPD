using System.Linq.Expressions;

namespace IPD.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> FindAllByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
       params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate,
       params Expression<Func<T, object>>[] includes);
    }
}
