using ecommerce_app.backend.web.Models.Auth;
using ecommerce_app.backend.web.Services.Auth;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ecommerce_app.backend.web.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.NameIdentifier, response.UserId.ToString()),
                new Claim(ClaimTypes.Name, response.Username),
                new Claim(ClaimTypes.Role, response.Role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, 
                CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity));

            return Ok(response);
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(200)]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
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

        [HttpGet]
        [Route("confirm-email")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Token is missing or invalid.");
            }

            await _authService.ConfirmEmailAsync(token, cancellationToken);

            return Content(@"
                <div style='text-align: center; margin-top: 50px; font-family: Arial, sans-serif;'>
                    <h2 style='color: #28a745;'>Email Confirmed Successfully!</h2>
                    <p>Your account is now active. You can close this tab and log in.</p>
                </div>", "text/html");
        }
    }
}
