namespace ecommerce_app.backend.web.Models.Order
{
    public class CreateOrderResponse
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? PaymentUrl { get; set; }
    }
}
