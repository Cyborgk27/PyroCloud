using PyroCloud.Core.Domain.Common;

namespace PyroCloud.Core.Domain.Entities.Identity
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string ShowName { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
