using Microsoft.AspNetCore.Http;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.Shared.Infrastructure.Services
{
    public class UrlService : IUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAbsoluteUrl(string relativePath)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null) return relativePath;

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";

            return $"{baseUrl}/{relativePath.TrimStart('/')}";
        }
    }
}
