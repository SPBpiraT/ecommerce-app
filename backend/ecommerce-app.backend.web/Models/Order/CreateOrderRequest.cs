using System.ComponentModel.DataAnnotations;

namespace ecommerce_app.backend.web.Models.Order
{
    public class CreateOrderRequest
    {
        [Required]
        [Range(0.01, 100000000, ErrorMessage = "Amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }
    }
}
