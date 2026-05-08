using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Application.Dtos.Auth;
using PyroCloud.Core.Application.Interfaces;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Exceptions;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Modules.Auth.Interfaces;
using PyroCloud.Shared.Infrastructure.Presistence.Context;

namespace PyroCloud.Modules.Auth.Services
{
    public class AuthAppService : IAuthAppService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly PyroDbContext _context;
        private readonly IFileAppService _file;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthAppService(IPasswordHasher passwordHasher, PyroDbContext context, IJwtTokenGenerator jwtTokenGenerator, IFileAppService file)
        {
            _passwordHasher = passwordHasher;
            _context = context;
            _jwtTokenGenerator = jwtTokenGenerator;
            _file = file;
        }
        public async Task<UserResponseDto> LoginAsync(LoginRequestDto input)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == input.Username);
            var isValid = !_passwordHasher.Verify(input.Password, user!.PasswordHash);
            
            if(user is null || isValid)
            {
                throw new UserFriendlyException("User or password is incorrect");
            }

            var token = _jwtTokenGenerator.GenerateToken(user);

            var userResponse = new UserResponseDto
            {
                Username = user.Username,
                ImageUrl = user.ImageUrl!,
                Token = token
            };

            return userResponse;
        }

        public async Task<string> RegisterAsync(RegisterDto input)
        {
            if (_context.Users.Any(u => u.Username == input.Username))
                throw new UserFriendlyException("Username already exists.");

            var imageUrl = await _file.SaveFileAsync(input.ImageUrl, "users");
            var passwordHash = _passwordHasher.Hash(input.Password);

            var user = new User
            {
                Username = input.Username,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
                ImageUrl = imageUrl,
                PasswordHash = passwordHash
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user.Id.ToString();
        }
    }
}
