namespace ProductManagement.Application.DTOs
{
    public class ProductCreateDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public string? CreatedBy { get; set; }
    }
}