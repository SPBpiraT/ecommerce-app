namespace ecommerce_app.backend.web.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
        Task SendConfirmationEmailAsync(string to, string rawToken, CancellationToken cancellationToken = default);
    }
}
