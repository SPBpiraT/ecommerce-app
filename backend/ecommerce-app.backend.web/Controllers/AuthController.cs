using ecommerce_app.backend.web.Models.Auth;
using ecommerce_app.backend.web.Services.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
    }
}
