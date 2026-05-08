namespace PyroCloud.Core.Application.Common
{
    public class PermissionDto
    {
        public string Label { get; set; } = default!;
        public string Value { get; set; } = default!;
        public List<PermissionDto> Children { get; set; } = new();

        public PermissionDto(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}
