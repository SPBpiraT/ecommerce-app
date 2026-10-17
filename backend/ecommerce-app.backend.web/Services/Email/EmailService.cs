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

        public async Task SendConfirmationEmailAsync(string to, string rawToken, CancellationToken cancellationToken = default)
        {
            var subject = "Confirm your registration.";

            var confirmationLink = $"https://localhost:7008/auth/confirm-email?token={rawToken}";

            var emailBody = $@"
                <h2>Welcome to EcommApp!</h2>
                <p>Thank you for registering. To confirm your email address, please click the link below:</p>
                <p><a href='{confirmationLink}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Confirm Email</a></p>
                <p>This link is valid for 24 hours.</p>";

            await SendEmailAsync(to, subject, emailBody, cancellationToken);
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
