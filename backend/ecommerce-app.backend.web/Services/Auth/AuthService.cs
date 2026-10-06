using ecommerce_app.backend.web.Services.User;
using ecommerce_app.backend.web.Models.Auth;
using ecommerce_app.backend.web.Models.User;
using ecommerce_app.backend.web.Exceptions;
using ecommerce_app.backend.web.Services.Email;
using ecommerce_app.backend.web.Common.Providers;
using ecommerce_app.backend.web.Entities;

using BCrypt.Net;
using BCryptNet = BCrypt.Net.BCrypt;
using System.Security.Cryptography;
using System.Text;

namespace ecommerce_app.backend.web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuthService(
            ILogger<AuthService> logger,
            IUserService userService,
            IEmailService emailService,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _userService = userService;
            _emailService = emailService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userService.GetByUsernameAsync(request.Username);

            if (user == null)
                throw new AuthException();

            var isOk = BCryptNet.Verify(request.Password, user.PasswordHash, false, HashType.SHA256);

            if (!isOk)
                throw new AuthException();

            if (!user.IsEmailConfirmed)
            {
                throw new AuthException("Please confirm your email address before logging in.");
            }

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

            var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

            var now = _dateTimeProvider.UtcNow;
            var expiresAt = now.AddHours(24);

            var confirmationToken = new EmailConfirmationToken
            {
                Id = Guid.NewGuid(),
                UserId = userModel.Id,
                TokenHash = tokenHash,
                ExpiresAt = expiresAt,
                Created = now
            };

            await _userService.SaveConfirmationTokenAsync(confirmationToken, cancellationToken);

            var confirmationLink = $"https://localhost:7008/auth/confirm-email?token={rawToken}";

            var emailBody = $@"
                <h2>Welcome to EcommApp!</h2>
                <p>Thank you for registering. To confirm your email address, please click the link below:</p>
                <p><a href='{confirmationLink}' style='padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>Confirm Email</a></p>
                <p>This link is valid for 24 hours.</p>";

            await _emailService.SendEmailAsync(request.Email, "Confirm your registration", emailBody, cancellationToken);

            return new RegisterResponse { UserId = userModel.Id };
        }

        public async Task ConfirmEmailAsync(string rawToken, CancellationToken cancellationToken = default)
        {
            var tokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(rawToken)));

            var tokenInfo = await _userService.GetTokenInfoAsync(tokenHash, cancellationToken);

            if (tokenInfo == null)
            {
                throw new AuthException("Invalid or expired confirmation token.");
            }

            if (tokenInfo.ExpiresAt < _dateTimeProvider.UtcNow)
            {
                throw new AuthException("Confirmation token has expired.");
            }

            var userModel = await _userService.GetByIdAsync(tokenInfo.UserId, cancellationToken);

            if (userModel == null)
            {
                throw new AuthException("User not found.");
            }

            await _userService.DeleteConfirmationTokenAsync(tokenInfo.Id, cancellationToken);

            await _userService.ConfirmEmailStatusAsync(userModel.Id, cancellationToken);
        }
    }
}
