namespace PyroCloud.Core.Application.Dtos.Setup
{
    public class SeedDataOptions
    {
        public TenantSeedDto DefaultTenant { get; set; } = null!;
        public List<RoleSeedDto> Roles { get; set; } = new();
        public List<UserSeedDto> Users { get; set; } = new();
    }

    public record TenantSeedDto(string Name, string Code, string Description);
    public record RoleSeedDto(string Name, string ShowName, string Description);
    public record UserSeedDto(string Username, string Email, string Password, List<string> Roles);
}
