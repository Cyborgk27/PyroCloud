namespace PyroCloud.Core.Application.Dtos.Auth
{
    public class UserResponseDto
    {
        public string Username { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string Token { get; set; } = default!;
        public List<string> Roles { get; set; } = new List<string>();
        public List<string> Permissions { get; set; } = new List<string>();

    }
}
