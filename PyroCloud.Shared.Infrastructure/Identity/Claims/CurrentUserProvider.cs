using Microsoft.AspNetCore.Http;
using PyroCloud.Core.Domain.Interfaces;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace PyroCloud.Shared.Infrastructure.Identity.Claims
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                return Guid.TryParse(claimValue, out var userIdGuid) ? userIdGuid : null;
            }
        }

        public Guid? TenantId
        {
            get
            {
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;
                return Guid.TryParse(claimValue, out var tenantId) ? tenantId : null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirst("userName")?.Value
                                ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public List<string> Roles
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindAll(ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList() ?? new List<string>();
            }
        }

        public List<string> Permissions
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?
                    .FindAll("permission")
                    .Select(c => c.Value)
                    .ToList() ?? new List<string>();
            }
        }
    }
}