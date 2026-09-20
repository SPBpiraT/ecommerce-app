using ecommerce_app.backend.web.Models.User;

namespace ecommerce_app.backend.web.Services.User
{
    public interface IUserService
    {
        Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserModel> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);

        Task<UserModel> CreateAsync(UserModel model, CancellationToken cancellationToken = default);
    }
}
