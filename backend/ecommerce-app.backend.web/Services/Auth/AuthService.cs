using ecommerce_app.backend.web.Services.User;
using ecommerce_app.backend.web.Models.Auth;
using ecommerce_app.backend.web.Models.User;
using ecommerce_app.backend.web.Exceptions;

using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;

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
            var user = await _userService.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new AuthException();

            var isOk = BCryptNet.Verify(request.Password, user.PasswordHash, false, HashType.SHA256);

            if (!isOk)
                throw new AuthException();

            return new AuthResponse
            {
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var usernameResult = await _userService.IsUsernameExistsAsync(request.Username, cancellationToken);

            if (usernameResult)
                throw new AuthException("Username already taken!");

            var passwordSalt = BCryptNet.GenerateSalt();
            var passwordHash = BCryptNet.HashPassword(request.Password, passwordSalt, enhancedEntropy: false, HashType.SHA256);

            var userModel = new UserModel
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            userModel = await _userService.CreateAsync(userModel, cancellationToken);

            return new RegisterResponse { UserId = userModel.Id };
        }
    }
}
