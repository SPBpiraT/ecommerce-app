using ecommerce_app.backend.web.Models.Payment;
using ecommerce_app.backend.web.Services.Order;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_app.backend.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public PaymentController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> HandleWebhook([FromBody] YooKassaWebhookNotification notification, CancellationToken cancellationToken = default)
        {
            if (notification.Event == "payment.succeeded" && notification.Object.Paid)
            {
                var paymentId = notification.Object.Id;

                var order = await _orderService.GetByPaymentIdAsync(paymentId, cancellationToken);

                if (order != null)
                {
                    await _orderService.UpdateStatusAsync(order.Id, "Paid", cancellationToken);
                }
            }

            return Ok();
        }
    }
}
