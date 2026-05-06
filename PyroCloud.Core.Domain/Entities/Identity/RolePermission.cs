using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class RolePermission : BaseEntity<int>
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = default!;
        public string Permission { get; set; } = default!;
    }
}
