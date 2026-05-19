namespace ProductManagement.Domain.Entities
{
    public partial class Category
    {
        public Category(string name, int minStockQuantity, string slug, string? description = null, Guid? parentId = null, string? createdBy = null)
        {
            SetName(name);
            SetMinStockQuantity(minStockQuantity);
            SetSlug(slug);
            SetDescription(description);

            ParentId = parentId;

            SetCreatedInfo(createdBy);
        }

        public void Update(string name,int minStockQuantity,string slug, string? description,Guid? parentId,string? updatedBy)
        {
            SetName(name);
            SetMinStockQuantity(minStockQuantity);
            SetSlug(slug);
            SetDescription(description);
            SetParent(parentId);

            SetUpdatedInfo(updatedBy);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Kategori adı boş veya geçersiz olamaz.", nameof(name));

            if (name.Length > 150)
                throw new ArgumentException("Kategori adı 150 karakterden uzun olamaz.", nameof(name));

            Name = name.Trim();
        }

        public void SetMinStockQuantity(int minStockQuantity)
        {
            if (minStockQuantity < 0)
                throw new ArgumentException("Minimum stok miktarı negatif olamaz.", nameof(minStockQuantity));

            MinStockQuantity = minStockQuantity;
        }

        public void SetSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Kategori adı boş olamaz.", nameof(slug));

            if (slug.Length > 200)
                throw new ArgumentException("Kategori  adı 200 karakterden uzun olamaz.", nameof(slug));

            Slug = slug.Trim().ToLowerInvariant();
        }

        public void SetDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = null;
                return;
            }

            if (description.Length > 500)
                throw new ArgumentException("Kategori açıklaması 500 karakterden uzun olamaz.", nameof(description));

            Description = description.Trim();
        }

        public void SetParent(Guid? parentId)
        {
            if (parentId.HasValue && parentId.Value == Id)
                throw new InvalidOperationException("Bir kategori kendisinin üst kategorisi olamaz.");

            ParentId = parentId;
        }
    }
}
