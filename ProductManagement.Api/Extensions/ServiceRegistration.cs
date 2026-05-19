using AutoMapper;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Concrete;
using ProductManagement.Application.Abstract;
using ProductManagement.Application.Mappings;
using ProductManagement.Repository.Abstract;
using ProductManagement.Repository.Concrete;
using ProductManagement.Repository.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace ProductManagement.Api.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(serviceProvider =>
            {
                var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); },
                serviceProvider.GetRequiredService<ILoggerFactory>());

                return configuration.CreateMapper();
            });

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
