namespace PyroCloud.Shared.Infrastructure.Common.Settings
{
    public class InfrastructureSettings
    {
        public EmailSettings Email { get; set; } = new();
        public StorageSettings Storage { get; set; } = new();
    }
}
