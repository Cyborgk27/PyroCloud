using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PyroCloud.Shared.Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly InfrastructureSettings _settings;

        public JwtTokenGenerator(IOptions<InfrastructureSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("userName", user.Username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Security.Jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Security.Jwt.Issuer,
                audience: _settings.Security.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.Security.Jwt.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
