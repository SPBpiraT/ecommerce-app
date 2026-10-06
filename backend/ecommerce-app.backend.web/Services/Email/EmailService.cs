using ecommerce_app.backend.web.Common.Configuration;

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ecommerce_app.backend.web.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", to));
            emailMessage.Subject = subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);

                await client.AuthenticateAsync(_smtpSettings.SenderEmail, _smtpSettings.Password, cancellationToken);

                await client.SendAsync(emailMessage, cancellationToken);
            }
            finally
            {
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
