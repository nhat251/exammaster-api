using System.Linq.Expressions;
using Common.Pagination;
namespace Domain.Interfaces
{
    public interface IGenericRepository<T, TKey> where T : class, IEntity<TKey>
    {
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        );
        Task<T?> GetAsync(
            Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes
        );

        Task<PageResult<T>> GetPagedAsync(
            Expression<Func<T, bool>>? predicate = null,
            PageRequest? pageRequest = null,
            params Expression<Func<T, object>>[] includes
        );
        Task<T> AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> items);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task<List<T>> GetAllByIdsAsync(IEnumerable<TKey> ids, params Expression<Func<T, object>>[] includes);
        Task<T?> GetByIdAsync(TKey id, params Expression<Func<T, object>>[] includes);
    }
}
