using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Authorization;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using PyroCloud.Shared.Infrastructure.Identity.Claims;
using PyroCloud.Shared.Infrastructure.Presistence.Context;
using PyroCloud.Shared.Infrastructure.Presistence.Interceptors;
using PyroCloud.Shared.Infrastructure.Services;
using PyroCloud.Shared.Infrastructure.Setup;
using System.Text;

namespace PyroCloud.Shared.Infrastructure.Extensions
{
    public static class InjectionExtension
    {
        public static IServiceCollection AddInjectionInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var infraSettings = new InfrastructureSettings();

            configuration.GetSection("Infrastructure").Bind(infraSettings);

            services.AddScoped<SeedDataService>();

            var jwtSettings = infraSettings.Security.Jwt;

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

            services.Configure<InfrastructureSettings>(configuration.GetSection("Infrastructure"));

            services.AddSingleton<IDateTime, DateTimeService>();

            services.AddTransient<IEmailService, SmtpEmailService>();

            services.AddScoped<IStorageService, LocalStorageService>();

            services.AddScoped<AuditInterceptor>();

            services.AddScoped<IUrlService, UrlService>();

            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddDbContext<PyroDbContext>((sp, options) =>
            {
                var auditInterceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(configuration.GetConnectionString(infraSettings.Database.SqlServer))
                       .AddInterceptors(auditInterceptor);
            });

            return services;
        }
    }
}
