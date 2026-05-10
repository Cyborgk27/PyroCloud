using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class Tenant : BaseEntity<Guid>
    {
        public string Name { get; set; } = default!;
        public string Code { get; set; } = default!;
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
