namespace PyroCloud.Core.Application.Common
{
    public class Permission
    {
        public string Name { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? ParentName { get; set; }
        public List<Permission> Children { get; set; } = new();

        public Permission(string name, string displayName, string? parentName = null)
        {
            Name = name;
            DisplayName = displayName;
            ParentName = parentName;
        }

        public void AddChild(Permission child)
        {
            child.ParentName = this.Name;
            Children.Add(child);
        }
    }
}
