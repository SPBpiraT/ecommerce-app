using ecommerce_app.backend.web.Entities;
using ecommerce_app.backend.web.Models.User;

namespace ecommerce_app.backend.web.Services.User
{
    public interface IUserService
    {
        Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserModel?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task<bool> IsUsernameExistsAsync(string username, CancellationToken cancellationToken = default);
        Task<UserModel> CreateAsync(UserModel model, CancellationToken cancellationToken = default);
        Task SaveConfirmationTokenAsync(EmailConfirmationToken token, CancellationToken cancellationToken = default);
        Task<EmailConfirmationToken?> GetTokenInfoAsync(string tokenHash, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(UserModel model, CancellationToken cancellationToken = default);
        Task ConfirmEmailStatusAsync(Guid userId, CancellationToken cancellationToken = default);
        Task DeleteConfirmationTokenAsync(Guid tokenId, CancellationToken cancellationToken = default);
    }
}
