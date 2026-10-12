using ecommerce_app.backend.web.Models.YooKassaClient;

namespace ecommerce_app.backend.web.Services.YooKassaClient
{
    public interface IYooKassaClient
    {
        Task<YooKassaPaymentResponse?> CreatePaymentAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default);
    }
}
