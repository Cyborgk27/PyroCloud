namespace PyroCloud.Core.Domain.Common
{
    public abstract class BaseEntity<TKey> : IAuditableEntity
    {
        public TKey Id { get; set; } = default!;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
