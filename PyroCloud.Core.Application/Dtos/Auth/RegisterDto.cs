using Microsoft.AspNetCore.Http;

namespace PyroCloud.Core.Application.Dtos.Auth
{
    public class RegisterDto
    {
        public string Username { get; set; } = default!;
        public string? Email { get; set; }
        public string Password { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IFormFile ImageUrl { get; set; } = default!;
    }
}
