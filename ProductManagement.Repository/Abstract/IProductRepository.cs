using ProductManagement.Domain.Entities;

namespace ProductManagement.Repository.Abstract
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product?> GetByIdWithCategoryAsync(Guid id, CancellationToken cancellationToken = default);

        Task<List<Product>> FilterAsync(string? keyword, int? minStockQuantity, int? maxStockQuantity, CancellationToken cancellationToken = default);
    }
}
