using ecommerce_app.backend.web.Models.Auth;
using ecommerce_app.backend.web.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_app.backend.web.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(typeof(AuthResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Authenticate(AuthRequest model)
        {
            var response = await _authService.AuthenticateAsync(model);
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(RegisterResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            var response = await _authService.RegisterAsync(model);
            return Ok(response);
        }
    }
}
