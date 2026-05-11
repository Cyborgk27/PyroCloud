namespace PyroCloud.Core.Domain.Interfaces
{
    public interface ICurrentUserProvider
    {
        Guid? UserId { get; }
        Guid? TenantId { get; }
        bool IsAuthenticated { get; }
        string? UserName { get; }
        List<string> Roles { get; }
        List<string> Permissions { get; }
    }
}
