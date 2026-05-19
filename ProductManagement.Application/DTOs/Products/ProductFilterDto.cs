namespace ProductManagement.Application.DTOs
{
    public class ProductFilterDto
    {
        public string? Keyword { get; set; }

        public int? MinStockQuantity { get; set; }

        public int? MaxStockQuantity { get; set; }

        public bool? IsLive { get; set; }

        public bool? IsActive { get; set; }
    }
}