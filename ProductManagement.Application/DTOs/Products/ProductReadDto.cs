namespace ProductManagement.Application.DTOs
{
    public class ProductReadDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public int CategoryMinStockQuantity { get; set; }

        public int StockQuantity { get; set; }

        public bool IsLive { get; set; }

        public bool IsActive { get; set; }

        public bool CanBeLive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }
}