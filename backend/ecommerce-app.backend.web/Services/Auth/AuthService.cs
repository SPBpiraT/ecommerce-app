using ecommerce_app.backend.web.Services.User;
using ecommerce_app.backend.web.Models.Auth;

namespace ecommerce_app.backend.web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserService _userService;

        public AuthService(
            ILogger<AuthService> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
