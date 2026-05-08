using Microsoft.AspNetCore.Http;
using PyroCloud.Core.Application.Dtos.Auth;

namespace PyroCloud.Modules.Auth.Interfaces
{
    public interface IAuthAppService
    {
        public Task<UserResponseDto> LoginAsync(LoginRequestDto input);
        public Task<string> RegisterAsync(RegisterDto input);
    }
}
