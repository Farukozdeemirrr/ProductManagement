using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration? _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options,IConfiguration configuration): base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var connectionString = _configuration?.GetConnectionString("DefaultConnection");


            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}