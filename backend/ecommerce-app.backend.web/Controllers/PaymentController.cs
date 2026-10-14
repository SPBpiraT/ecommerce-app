using ecommerce_app.backend.web.Common.Configuration;
using ecommerce_app.backend.web.Models.Payment;
using ecommerce_app.backend.web.Services.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace ecommerce_app.backend.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly List<IPNetwork> _allowedNetworks;

        public PaymentController(IOrderService orderService,
            IOptions<YooKassaSettings> settings)
        {
            _orderService = orderService;

            _allowedNetworks = settings.Value.AllowedWebhookNetworks
                .Select(IPNetwork.Parse)
                .ToList();
        }

        [HttpPost]
        [Route("webhook")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> HandleWebhook([FromBody] YooKassaWebhookNotification notification, CancellationToken cancellationToken = default)
        {
            var remoteIp = HttpContext.Connection.RemoteIpAddress;

            if (remoteIp == null)
            {
                return Forbid("Unable to determine remote IP address.");
            }

            if (!IPAddress.IsLoopback(remoteIp))
            {
                bool isAllowed = _allowedNetworks.Any(network => network.Contains(remoteIp));

                if (!isAllowed)
                {
                    return Forbid("Access denied. IP address is not recognized as a valid YooKassa server.");
                }
            }

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
