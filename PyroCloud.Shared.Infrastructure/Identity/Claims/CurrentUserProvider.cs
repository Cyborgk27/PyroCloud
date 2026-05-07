using Microsoft.AspNetCore.Http;
using PyroCloud.Core.Domain.Interfaces;
using System.Security.Claims;

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
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(claimValue, out var userIdGuid))
                {
                    return userIdGuid;
                }

                return null;
            }
        }

        public Guid? TenantId
        {
            get
            {
                var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id")?.Value;
                return Guid.TryParse(claimValue, out var tenantId) ? tenantId : null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
