namespace ProductManagement.Application.DTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = null!;

        public int MinStockQuantity { get; set; }

        public Guid? ParentId { get; set; }

        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public string? CreatedBy { get; set; }
    }
}