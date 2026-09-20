using ecommerce_app.backend.web.Models.User;

namespace ecommerce_app.backend.web.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        //TODO: Add Context

        public UserService(
            ILogger<UserService> logger) //TODO: Add Context
        {
            _logger = logger;
        }

        public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> CreateAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
