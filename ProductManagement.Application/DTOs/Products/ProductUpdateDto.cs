namespace ProductManagement.Application.DTOs
{
    public class ProductUpdateDto
    {
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        public int StockQuantity { get; set; }

        public string? UpdatedBy { get; set; }
    }
}
