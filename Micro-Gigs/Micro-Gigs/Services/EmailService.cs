using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Micro_Gigs.Services
{
    public class EmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration configuration)
        {
            config = configuration;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var section = config.GetSection("EmailSettings");
                var host = section["Host"];
                var portStr = section["Port"];
                var username = section["Username"];
                var password = section["Password"];
                var from = section["From"] ?? username;

                if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(portStr) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    // Missing configuration
                    return false;
                }

                int port = int.Parse(portStr);

                using var message = new MailMessage(from, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                using var client = new SmtpClient(host, port)
                {
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 20000
                };

                await client.SendMailAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
