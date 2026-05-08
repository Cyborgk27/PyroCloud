using Microsoft.AspNetCore.Mvc;
using PyroCloud.Core.Application.Dtos.Auth;
using PyroCloud.Modules.Auth.Interfaces;

namespace PyroCloud.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthAppService _authAppService;

        public AuthController(IAuthAppService authAppService)
        {
            _authAppService = authAppService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authAppService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto request)
        {
            var result = await _authAppService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
