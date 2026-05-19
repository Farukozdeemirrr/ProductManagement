using AutoMapper;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryReadDto>().ForMember(
                    destination => destination.ParentName,
                    option => option.MapFrom(source =>source.Parent != null ? source.Parent.Name: null));

            CreateMap<Product, ProductReadDto>()
                .ForMember(destination => destination.CategoryName,
                    option => option.MapFrom(source => source.Category != null ? source.Category.Name : string.Empty))
                .ForMember(
                    destination => destination.CategoryMinStockQuantity,
                    option => option.MapFrom(source => source.Category != null ? source.Category.MinStockQuantity : 0))
                .ForMember(
                    destination => destination.CanBeLive,
                    option => option.MapFrom(source => source.Category != null
                        && source.Category.IsActive && source.StockQuantity >= source.Category.MinStockQuantity));
        }
    }
}
