using ecommerce_app.backend.web.Models.Payment;

namespace ecommerce_app.backend.web.Services.Payment
{
    public interface IYooKassaClient
    {
        Task<YooKassaPaymentResponse?> CreatePaymentAsync(Guid orderId, decimal amount, CancellationToken cancellationToken = default);
    }
}
