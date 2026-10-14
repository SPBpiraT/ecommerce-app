using ecommerce_app.backend.web.Common.Providers;
using ecommerce_app.backend.web.Entities;
using ecommerce_app.backend.web.Models.Order;
using ecommerce_app.backend.web.Services.Order;
using ecommerce_app.backend.web.Services.YooKassaClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerce_app.backend.web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IYooKassaClient _yooKassaClient;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OrderController(
            IOrderService orderService,
            IYooKassaClient yooKassaClient,
            IDateTimeProvider dateTimeProvider)
        {
            _orderService = orderService;
            _yooKassaClient = yooKassaClient;
            _dateTimeProvider = dateTimeProvider;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CreateOrderResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User is not authorized or the session has expired.");
            }

            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                UserId = userId,
                TotalAmount = request.TotalAmount,
                Status = "Pending",
                PaymentId = null,
                Created = _dateTimeProvider.UtcNow
            };

            await _orderService.CreateAsync(order, cancellationToken);

            try
            {
                var yooKassaResponse = await _yooKassaClient.CreatePaymentAsync(orderId, order.TotalAmount, cancellationToken);

                if (yooKassaResponse == null || yooKassaResponse.Confirmation == null)
                {
                    return BadRequest("Failed to initiate payment via YooKassa.");
                }

                await _orderService.UpdatePaymentInfoAsync(orderId, yooKassaResponse.Id, cancellationToken);

                var response = new CreateOrderResponse
                {
                    OrderId = orderId,
                    Status = order.Status,
                    PaymentUrl = yooKassaResponse.Confirmation.ConfirmationUrl
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _orderService.UpdateStatusAsync(orderId, "Canceled", cancellationToken);
                return BadRequest($"Payment system error: {ex.Message}");
            }
        }
    }
}
