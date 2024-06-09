using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure
{
    public class EmailSender : ISendEmail
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            string MailServer = configuration["EmailSettings:MailServer"] ?? "";
            string FromEmail = configuration["EmailSettings:FromEmail"] ?? "";
            string Password = configuration["EmailSettings:Password"] ?? "";
            int Port = int.Parse(configuration["EmailSettings:MailPort"] ?? "");

            var client = new SmtpClient(MailServer, Port)
            {
                Credentials = new NetworkCredential(FromEmail, Password),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage(FromEmail, toEmail, subject, body)
            {
                IsBodyHtml = isBodyHtml
            };

            return client.SendMailAsync(mailMessage);
        }
    }
}