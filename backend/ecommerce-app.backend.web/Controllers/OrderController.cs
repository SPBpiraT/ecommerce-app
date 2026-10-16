using ecommerce_app.backend.web.Models.Order;
using ecommerce_app.backend.web.Services.Order;
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

        public OrderController(IOrderService orderService) => _orderService = orderService;

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(typeof(CreateOrderResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User is not authorized or the session has expired.");
            }

            var response = await _orderService.CheckoutAsync(request, userId, cancellationToken);

            return Ok(response);
        }
    }
}
