using ecommerce_app.backend.web.Models.Auth;

namespace ecommerce_app.backend.web.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest request, CancellationToken cancellationToken = default);

        Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    }
}
