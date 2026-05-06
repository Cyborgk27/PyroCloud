using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class Company : BaseEntity<Guid>, ITenantEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public Guid? TenantId { get; set; }
        public virtual Tenant? Tenant { get; set; }
        public virtual ICollection<UserCompany> AllowedUsers { get; set; } = new List<UserCompany>();
    }
}
