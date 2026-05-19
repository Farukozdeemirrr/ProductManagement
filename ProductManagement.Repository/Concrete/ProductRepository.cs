using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Context;
using ProductManagement.Repository.Abstract;

namespace ProductManagement.Repository.Concrete
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetByIdWithCategoryAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await Query()
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Product>> FilterAsync(string? keyword, int? minStockQuantity, int? maxStockQuantity,
            CancellationToken cancellationToken = default)
        {
            var query = Query().Include(x => x.Category).Where(x => x.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var normalizedKeyword = keyword.Trim().ToLower();

                query = query.Where(x =>
                    x.Title.ToLower().Contains(normalizedKeyword) ||
                    (x.Description != null && x.Description.ToLower().Contains(normalizedKeyword)) ||
                    x.Category.Name.ToLower().Contains(normalizedKeyword));
            }

            if (minStockQuantity.HasValue)
            {
                query = query.Where(x => x.StockQuantity >= minStockQuantity.Value);
            }

            if (maxStockQuantity.HasValue)
            {
                query = query.Where(x => x.StockQuantity <= maxStockQuantity.Value);
            }

            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);
        }
    }
}
