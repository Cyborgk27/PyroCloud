using Microsoft.Extensions.Options;
using PyroCloud.Core.Domain.Interfaces;
using PyroCloud.Shared.Infrastructure.Common.Settings;
using System.Net;
using System.Net.Mail;

namespace PyroCloud.Shared.Infrastructure.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public SmtpEmailService(IOptions<InfrastructureSettings> options)
        {
            _settings = options.Value.Email;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port)
            {
                Credentials = new NetworkCredential(_settings.UserName, _settings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(_settings.FromEmail, to, subject, body);
            await client.SendMailAsync(mailMessage);
        }
    }
}
