using ProductManagement.Domain.Common;

namespace ProductManagement.Domain.Entities
{
    public partial class Category : AuditableEntity
    {
        private readonly List<Category> _children = new();

        private Category()
        {
        }

        public string Name { get; private set; } = null!;

        public int MinStockQuantity { get; private set; }

        public Guid? ParentId { get; private set; }

        public Category? Parent { get; private set; }

        public string Slug { get; private set; } = null!;

        public string? Description { get; private set; }

        public IReadOnlyCollection<Category> Children => _children.AsReadOnly();
    }
}