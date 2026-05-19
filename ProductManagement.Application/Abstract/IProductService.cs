using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Abstract
{
    public interface IProductService
    {
        Task<ResponseDto<ProductReadDto>> CreateAsync(ProductCreateDto createDto, CancellationToken cancellationToken = default);

        Task<ResponseDto<ProductReadDto>> UpdateAsync(Guid id, ProductUpdateDto updateDto, CancellationToken cancellationToken = default);

        Task<ResponseDto<ProductReadDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ResponseDto<List<ProductReadDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ResponseDto<List<ProductReadDto>>> FilterAsync(ProductFilterDto filterDto, CancellationToken cancellationToken = default);

        Task<ResponseDto<ProductReadDto>> ToggleLiveStatusAsync(Guid id,string? updatedBy, CancellationToken cancellationToken = default);

        Task<ResponseDto<bool>> ToggleActiveStatusAsync(Guid id, string? updatedBy, CancellationToken cancellationToken = default);
    }
}
