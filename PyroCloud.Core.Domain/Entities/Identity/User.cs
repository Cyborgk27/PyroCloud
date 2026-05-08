using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class User : BaseEntity<Guid>, ITenantEntity
    {
        public string Username { get; set; } = default!;
        public string? Email { get; set; }
        public string PasswordHash { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ImageUrl { get; set; }
        public Guid? TenantId { get; set; }
        public Tenant? Tenant { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserCompany> AssignedCompanies { get; set; } = new List<UserCompany>();

    }
}
