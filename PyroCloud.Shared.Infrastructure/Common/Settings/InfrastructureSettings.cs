namespace PyroCloud.Shared.Infrastructure.Common.Settings
{
    public class InfrastructureSettings
    {
        public EmailSettings Email { get; set; } = new();
        public StorageSettings Storage { get; set; } = new();
        public DatabaseSetting Database { get; set; } = new();
        public SecuritySettings Security { get; set; } = new();
        public CorsSettings Cors { get; set; } = new();
    }
}
