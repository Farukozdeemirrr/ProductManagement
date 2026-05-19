using FluentValidation;
using ProductManagement.Application.DTOs;

namespace ProductManagement.Application.Validators
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Kategori adı boş veya null olamaz.")
                .MaximumLength(150)
                .WithMessage("Kategori adı 150 karakterden uzun olamaz.");

            RuleFor(x => x.MinStockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Minimum stok miktarı negatif olamaz.");

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Kategori kısa adı boş veya null olamaz.")
                .MaximumLength(200)
                .WithMessage("Kategori kısa adı 200 karakterden uzun olamaz.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Kategori açıklaması 500 karakterden uzun olamaz.")
                .When(x => !string.IsNullOrWhiteSpace(x.Description));
        }
    }
}
