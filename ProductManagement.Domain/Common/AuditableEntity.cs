namespace ProductManagement.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public bool IsActive { get; protected set; } = true;
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public string? CreatedBy { get; protected set; }

        public DateTime? UpdatedAt { get; protected set; }
        public string? UpdatedBy { get; protected set; }

        public DateTime? DeletedAt { get; protected set; }
        public string? DeletedBy { get; protected set; }

        protected void SetCreatedInfo(string? createdBy)
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        protected void SetUpdatedInfo(string? updatedBy)
        {
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updatedBy;
        }

        public void SetDeletedInfo(bool isActive, string? updatedBy)
        {
            IsActive = isActive;

            if (isActive)
            {
                DeletedAt = null;
                DeletedBy = null;
            }
            else
            {
                DeletedAt = DateTime.UtcNow;
                DeletedBy = updatedBy;
            }

            SetUpdatedInfo(updatedBy);
        }
    }
}
