using ProductManagement.Domain.Entities;

namespace ProductManagement.Repository.Abstract
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    }
}
