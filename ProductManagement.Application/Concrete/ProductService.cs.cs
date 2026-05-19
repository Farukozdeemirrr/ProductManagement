using AutoMapper;
using ProductManagement.Application.Abstract;
using ProductManagement.Application.DTOs;
using ProductManagement.Domain.Entities;
using ProductManagement.Repository.Abstract;
using ProductManagement.Repository.UnitOfWork;

namespace ProductManagement.Application.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository,ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto<ProductReadDto>> CreateAsync(ProductCreateDto createDto, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(createDto.CategoryId, cancellationToken);

            if (category is null)
                return ResponseDto<ProductReadDto>.Fail("Kategori bulunamadı.");

            if (!category.IsActive)
                return ResponseDto<ProductReadDto>.Fail("Aktif olmayan kategoriyle ürün oluşturulamaz.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var product = new Product(createDto.Title, createDto.Description, createDto.CategoryId,
                                            createDto.StockQuantity, createDto.CreatedBy);

                await _productRepository.AddAsync(product, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var productWithCategory = await _productRepository.GetByIdWithCategoryAsync(product.Id, cancellationToken);

                var productDto = _mapper.Map<ProductReadDto>(productWithCategory);

                return ResponseDto<ProductReadDto>.Success(productDto, "Ürün başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<ProductReadDto>.Fail(ex.Message);
            }
        }

        public async Task<ResponseDto<ProductReadDto>> UpdateAsync(Guid id, ProductUpdateDto updateDto,CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdAsync(id, cancellationToken);

            if (product is null)
                return ResponseDto<ProductReadDto>.Fail("Ürün bulunamadı.");

            var category = await _categoryRepository.GetByIdAsync(updateDto.CategoryId, cancellationToken);

            if (category is null)
                return ResponseDto<ProductReadDto>.Fail("Kategori bulunamadı.");

            if (!category.IsActive)
                return ResponseDto<ProductReadDto>.Fail("Ürün, pasif kategoriye atanamaz.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                product.Update(updateDto.Title, updateDto.Description, updateDto.CategoryId, updateDto.StockQuantity, updateDto.UpdatedBy);

                if (product.IsLive && !product.CanBeLive(category))
                {
                    product.IsLiveStatus(category, updateDto.UpdatedBy);
                }

                _productRepository.Update(product);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var productWithCategory = await _productRepository.GetByIdWithCategoryAsync(product.Id, cancellationToken);

                var productDto = _mapper.Map<ProductReadDto>(productWithCategory);

                return ResponseDto<ProductReadDto>.Success(productDto, "Ürün başarıyla güncellendi.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<ProductReadDto>.Fail(ex.Message);
            }
        }

        public async Task<ResponseDto<ProductReadDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdWithCategoryAsync(id, cancellationToken);

            if (product is null)
                return ResponseDto<ProductReadDto>.Fail("Ürün bulunamadı.");

            var productDto = _mapper.Map<ProductReadDto>(product);

            return ResponseDto<ProductReadDto>.Success(productDto);
        }

        public async Task<ResponseDto<List<ProductReadDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var products = await _productRepository.FilterAsync(
                keyword: null,
                minStockQuantity: null,
                maxStockQuantity: null,
                cancellationToken);

            var productDtos = _mapper.Map<List<ProductReadDto>>(products);

            return ResponseDto<List<ProductReadDto>>.Success(productDtos);
        }

        public async Task<ResponseDto<List<ProductReadDto>>> FilterAsync(
            ProductFilterDto filterDto,
            CancellationToken cancellationToken = default)
        {
            if (filterDto.MinStockQuantity.HasValue && filterDto.MaxStockQuantity.HasValue 
                && filterDto.MinStockQuantity.Value > filterDto.MaxStockQuantity.Value)
            {
                return ResponseDto<List<ProductReadDto>>.Fail("Minimum stok miktarı, maksimum stok miktarından fazla olamaz.");
            }

            var products = await _productRepository.FilterAsync(
                filterDto.Keyword,
                filterDto.MinStockQuantity,
                filterDto.MaxStockQuantity,
                cancellationToken);

            if (filterDto.IsLive.HasValue)
            {
                products = products.Where(x => x.IsLive == filterDto.IsLive.Value).ToList();
            }

            if (filterDto.IsActive.HasValue)
            {
                products = products.Where(x => x.IsActive == filterDto.IsActive.Value).ToList();
            }

            var productDtos = _mapper.Map<List<ProductReadDto>>(products);

            return ResponseDto<List<ProductReadDto>>.Success(productDtos);
        }

        public async Task<ResponseDto<ProductReadDto>> ToggleLiveStatusAsync(Guid id,string? updatedBy, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdWithCategoryAsync(id, cancellationToken);

            if (product is null)
                return ResponseDto<ProductReadDto>.Fail("Ürün bulunamadı.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                product.IsLiveStatus(product.Category, updatedBy);

                _productRepository.Update(product);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var productDto = _mapper.Map<ProductReadDto>(product);

                return ResponseDto<ProductReadDto>.Success(productDto, "Ürün canlı durumu başarıyla değiştirildi.");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return ResponseDto<ProductReadDto>.Fail(ex.Message);
            }
        }

        public async Task<ResponseDto<bool>> ToggleActiveStatusAsync(Guid id, string? updatedBy, CancellationToken cancellationToken = default)
        {
            var product = await _productRepository.GetByIdWithCategoryAsync(id, cancellationToken);

            if (product is null)
                return ResponseDto<bool>.Fail("Ürün bulunamadı.");

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var nextIsActive = !product.IsActive;

                if (!nextIsActive && product.IsLive)
                {
                    product.IsLiveStatus(product.Category, updatedBy);
                }

                product.SetDeletedInfo(nextIsActive, updatedBy);

                _productRepository.Update(product);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var message = nextIsActive
                    ? "Ürün başarıyla aktif edildi."
                    : "Ürün başarıyla pasife alındı.";

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
