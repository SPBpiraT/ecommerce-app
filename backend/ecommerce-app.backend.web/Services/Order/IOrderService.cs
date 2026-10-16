using ecommerce_app.backend.web.Models.Order;

namespace ecommerce_app.backend.web.Services.Order
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CheckoutAsync(CreateOrderRequest request, Guid userId, CancellationToken cancellationToken = default);
        Task CreateAsync(Entities.Order order, CancellationToken cancellationToken = default);
        Task UpdatePaymentInfoAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default);
        Task<Entities.Order?> GetByPaymentIdAsync(string paymentId, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(Guid orderId, string status, CancellationToken cancellationToken = default);
    }
}
