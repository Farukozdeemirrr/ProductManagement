using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class ProductUpdateDtoValidator : AbstractValidator<ProductUpdateDto>
    {
        public ProductUpdateDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Ürün başlığı boş veya null olamaz.")
                .MaximumLength(200)
                .WithMessage("Ürün başlığı 200 karakterden uzun olamaz.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("Ürünün bir kategorisi olmalıdır.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stok miktarı negatif olamaz.");
        }
    }
}
