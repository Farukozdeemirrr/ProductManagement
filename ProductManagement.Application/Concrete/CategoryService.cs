using AutoMapper;
using ProductManagement.Application.Abstract;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;
using ProductManagement.Repository.Abstract;
using ProductManagement.Repository.UnitOfWork;

namespace ProductManagement.Application.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<CategoryReadDto>> CreateAsync(CategoryCreateDto createDto, CancellationToken cancellationToken = default)
        {
            var existingCategory = await _categoryRepository.GetBySlugAsync(createDto.Slug, cancellationToken);

            if (existingCategory is not null)
                return ResponseDto<CategoryReadDto>.Fail("Kategori adı zaten mevcut.");

            if (createDto.ParentId.HasValue)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(createDto.ParentId.Value, cancellationToken);

                if (parentCategory is null)
                    return ResponseDto<CategoryReadDto>.Fail("Üst kategori bulunamadı.");

                if (!parentCategory.IsActive)
                    return ResponseDto<CategoryReadDto>.Fail("Üst kategori aktif değil.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var category = new Category(createDto.Name, createDto.MinStockQuantity, createDto.Slug,
                                                createDto.Description, createDto.ParentId,createDto.CreatedBy);

                await _categoryRepository.AddAsync(category, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var categoryDto = _mapper.Map<CategoryReadDto>(category);

                return ResponseDto<CategoryReadDto>.Success(categoryDto, "Kategori başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<CategoryReadDto>.Fail(ex.Message);
            }
        }

        public async Task<ResponseDto<CategoryReadDto>> UpdateAsync(Guid id, CategoryUpdateDto updateDto, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                return ResponseDto<CategoryReadDto>.Fail("Kategori bulunamadı.");

            var existingCategory = await _categoryRepository.GetBySlugAsync(updateDto.Slug, cancellationToken);

            if (existingCategory is not null && existingCategory.Id != id)
                return ResponseDto<CategoryReadDto>.Fail("Kategori adı zaten mevcut.");

            if (updateDto.ParentId.HasValue)
            {
                if (updateDto.ParentId.Value == id)
                    return ResponseDto<CategoryReadDto>.Fail("Bir kategori kendi kendisinin kategorisi olamaz.");

                var parentCategory = await _categoryRepository.GetByIdAsync(
                    updateDto.ParentId.Value,
                    cancellationToken);

                if (parentCategory is null)
                    return ResponseDto<CategoryReadDto>.Fail("Üst kategori bulunamadı.");

                if (!parentCategory.IsActive)
                    return ResponseDto<CategoryReadDto>.Fail("Üst kategori aktif değil.");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                category.Update(updateDto.Name, updateDto.MinStockQuantity, updateDto.Slug,
                                    updateDto.Description, updateDto.ParentId, updateDto.UpdatedBy);

                _categoryRepository.Update(category);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var categoryDto = _mapper.Map<CategoryReadDto>(category);

                return ResponseDto<CategoryReadDto>.Success(categoryDto, "Kategori başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<CategoryReadDto>.Fail(ex.Message);
            }
        }

        public async Task<ResponseDto<CategoryReadDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                return ResponseDto<CategoryReadDto>.Fail("Kategori bulunamadı.");

            var categoryDto = _mapper.Map<CategoryReadDto>(category);

            return ResponseDto<CategoryReadDto>.Success(categoryDto);
        }

        public async Task<ResponseDto<List<CategoryReadDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);

            var categoryDtos = _mapper.Map<List<CategoryReadDto>>(categories);

            return ResponseDto<List<CategoryReadDto>>.Success(categoryDtos);
        }

        public async Task<ResponseDto<bool>> ToggleActiveStatusAsync(Guid id, string? updatedBy, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category is null)
                return ResponseDto<bool>.Fail("Kategori bulunamadı.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var nextIsActive = !category.IsActive;

                category.SetDeletedInfo(nextIsActive, updatedBy);

                _categoryRepository.Update(category);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var message = nextIsActive
                    ? "Kategori başarıyla aktif edildi."
                    : "Kategori başarıyla pasife alındı.";

                return ResponseDto<bool>.Success(true, message);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<bool>.Fail(ex.Message);
            }
        }
    }
}
