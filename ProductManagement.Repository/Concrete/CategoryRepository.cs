using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure.Context;
using ProductManagement.Repository.Abstract;

namespace ProductManagement.Repository.Concrete
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Category?> GetBySlugAsync(string slug,CancellationToken cancellationToken = default)
        {
            var normalizedSlug = slug.Trim().ToLower();

            return await Query().FirstOrDefaultAsync(x => x.Slug == normalizedSlug, cancellationToken);
        }
    }
}
