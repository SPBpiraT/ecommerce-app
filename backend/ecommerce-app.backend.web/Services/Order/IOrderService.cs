
namespace ecommerce_app.backend.web.Services.Order
{
    public interface IOrderService
    {
        Task<Entities.Order> CreateAsync(Entities.Order order, CancellationToken cancellationToken = default);
        Task UpdatePaymentInfoAsync(Guid orderId, string paymentId, CancellationToken cancellationToken = default);
        Task<Entities.Order?> GetByPaymentIdAsync(string paymentId, CancellationToken cancellationToken = default);
        Task UpdateStatusAsync(Guid orderId, string status, CancellationToken cancellationToken = default);
    }
}
