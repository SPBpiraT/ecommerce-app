
namespace ecommerce_app.backend.web.Entities
{
    public class Order : IAuditEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public string? PaymentId { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime Created { get; set; }
    }
}
