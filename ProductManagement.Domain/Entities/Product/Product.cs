using ProductManagement.Domain.Common;

namespace ProductManagement.Domain.Entities
{
    public partial class Product : AuditableEntity
    {
        private Product()
        {
        }

        public string Title { get; private set; } = null!;

        public string? Description { get; private set; }

        public Guid CategoryId { get; private set; }

        public Category Category { get; private set; } = null!;

        public int StockQuantity { get; private set; }

        public bool IsLive { get; private set; }
    }
}
