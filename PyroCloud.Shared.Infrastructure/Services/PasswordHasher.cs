using Microsoft.Extensions.Options;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using BC = BCrypt.Net.BCrypt;

namespace PyroCloud.Shared.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _workFactor;
        public PasswordHasher(IOptions<InfrastructureSettings> settings)
        {
            _workFactor = settings.Value.Security.HashWorkFactor;
        }
        public string Hash(string password)
        {
            return BC.HashPassword(password, _workFactor);
        }

        public bool Verify(string password, string passwordHash)
        {
            return BC.Verify(password, passwordHash);
        }
    }
}
