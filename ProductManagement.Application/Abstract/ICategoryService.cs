using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Abstract
{
    public interface ICategoryService
    {
        Task<ResponseDto<CategoryReadDto>> CreateAsync(CategoryCreateDto createDto, CancellationToken cancellationToken = default);

        Task<ResponseDto<CategoryReadDto>> UpdateAsync(Guid id, CategoryUpdateDto updateDto, CancellationToken cancellationToken = default);

        Task<ResponseDto<CategoryReadDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<ResponseDto<List<CategoryReadDto>>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ResponseDto<bool>> ToggleActiveStatusAsync(Guid id, string? updatedBy, CancellationToken cancellationToken = default);
    }
}
