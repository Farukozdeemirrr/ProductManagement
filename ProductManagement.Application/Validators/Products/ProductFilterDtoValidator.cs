using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class ProductFilterDtoValidator : AbstractValidator<ProductFilterDto>
    {
        public ProductFilterDtoValidator()
        {
            RuleFor(x => x.Keyword)
                .MaximumLength(200)
                .WithMessage("Anahtar kelime 200 karakterden uzun olamaz.")
                .When(x => !string.IsNullOrWhiteSpace(x.Keyword));

            RuleFor(x => x.MinStockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum stok miktarı negatif olamaz.")
                .When(x => x.MinStockQuantity.HasValue);

            RuleFor(x => x.MaxStockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Maksimum stok miktarı negatif olamaz.")
                .When(x => x.MaxStockQuantity.HasValue);

            RuleFor(x => x)
                .Must(x =>!x.MinStockQuantity.HasValue ||!x.MaxStockQuantity.HasValue ||  x.MinStockQuantity.Value <= x.MaxStockQuantity.Value)
                .WithMessage("Minimum stok miktarı, maksimum stok miktarından büyük olamaz.");
        }
    }
}