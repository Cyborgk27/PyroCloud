using PyroCloud.Core.Domain.Entities.Identity;

namespace PyroCloud.Core.Application.Dtos.Identity.Roles
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ShowName { get; set; } = default!;
        public string Description { get; set; } = default!;
        //public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public IEnumerable<string> Permissions { get; set; } = new List<string>();
    }
}
