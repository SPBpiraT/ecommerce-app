using ecommerce_app.backend.web.Models.User;
using ecommerce_app.backend.web.Services.User;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce_app.backend.web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("profile")]
        [ProducesResponseType(typeof(GetUserProfileResponse), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProfile(CancellationToken cancellationToken = default)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("User is not authorized or session has expired.");
            }

            var profile = await _userService.GetUserProfileAsync(userId, cancellationToken);

            if (profile == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(profile);
        }
    }
}
