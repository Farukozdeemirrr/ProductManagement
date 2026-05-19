using ProductManagement.Domain.Common;
using System.Linq.Expressions;

namespace ProductManagement.Repository.Abstract
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        IQueryable<T> Query();

        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        void Update(T entity);

        void Delete(T entity);
    }
}
