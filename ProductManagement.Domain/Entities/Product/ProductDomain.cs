namespace ProductManagement.Domain.Entities
{
    public partial class Product
    {
        public Product(string title,string? description,Guid categoryId,int stockQuantity,string? createdBy = null)
        {
            SetTitle(title);
            SetDescription(description);
            SetCategory(categoryId);
            SetStockQuantity(stockQuantity);

            IsLive = false;

            SetCreatedInfo(createdBy);
        }

        public void Update(string title, string? description, Guid categoryId,int stockQuantity, string? updatedBy)
        {
            SetTitle(title);
            SetDescription(description);
            SetCategory(categoryId);
            SetStockQuantity(stockQuantity);

            SetUpdatedInfo(updatedBy);
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Ürün başlığı boş olamaz.", nameof(title));

            if (title.Length > 200)
                throw new ArgumentException("Ürün başlığı 200 karakterden uzun olamaz.", nameof(title));

            Title = title.Trim();
        }

        public void SetDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = null;
                return;
            }

            if (description.Length > 1000)
                throw new ArgumentException("Ürün açıklaması 1000 karakterden uzun olamaz.", nameof(description));

            Description = description.Trim();
        }

        public void SetCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Ürünün geçerli bir kategoriye sahip olması gerekir.", nameof(categoryId));

            CategoryId = categoryId;
        }

        public void SetStockQuantity(int stockQuantity)
        {
            if (stockQuantity < 0)
                throw new ArgumentException("Stok miktarı negatif olamaz.", nameof(stockQuantity));

            StockQuantity = stockQuantity;
        }

        public void IsLiveStatus(Category category, string? updatedBy)
        {
            if (!IsLive)
            {
                ValidateCanBeLive(category);
            }

            IsLive = !IsLive;

            SetUpdatedInfo(updatedBy);
        }

        public bool CanBeLive(Category category)
        {
            if (category is null)
                return false;

            if (CategoryId == Guid.Empty)
                return false;

            if (CategoryId != category.Id)
                return false;

            if (!category.IsActive)
                return false;

            if (StockQuantity < category.MinStockQuantity)
                return false;

            return true;
        }

        private void ValidateCanBeLive(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category), "Ürünün yayında olabilmesi için bir kategorisi olmalıdır.");

            if (CategoryId == Guid.Empty)
                throw new InvalidOperationException("Ürünün yayında olabilmesi için bir kategorisi olmalıdır.");

            if (CategoryId != category.Id)
                throw new InvalidOperationException("Ürün kategorisi verilen kategori ile eşleşmiyor.");

            if (!category.IsActive)
                throw new InvalidOperationException("Kategori aktif olmadığı için ürün yayına alınamaz.");

            if (StockQuantity < category.MinStockQuantity)
                throw new InvalidOperationException("Stok miktarı, kategorinin minimum stok miktarının altında olduğu için ürün yayına alınamaz.");
        }
    }
}
