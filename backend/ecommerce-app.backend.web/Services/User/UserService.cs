using ecommerce_app.backend.web.Mapping;
using ecommerce_app.backend.web.Models.User;
using System.Data;
using Dapper;
using ecommerce_app.backend.web.Entities;
using ecommerce_app.backend.web.Common.Providers;

namespace ecommerce_app.backend.web.Services.User
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IDbConnection _dbConnection;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(
            ILogger<UserService> logger,
            IDbConnection dbConnection,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _dbConnection = dbConnection;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<UserModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserModel> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUsernameExistsAsync(string username, CancellationToken cancellationToken = default)
        {
            const string sql = "SELECT EXISTS(SELECT 1 FROM users WHERE username = @Username AND is_active = true)";

            return await _dbConnection.QueryFirstOrDefaultAsync<bool>(new CommandDefinition(
                sql,
                new { Username = username },
                cancellationToken: cancellationToken));
        }

        public async Task<UserModel> CreateAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            var userId = Guid.NewGuid();
            var userInfoId = Guid.NewGuid();
            var now = _dateTimeProvider.UtcNow;

            var user = new Entities.User
            {
                Id = userId,
                Created = now,
                Username = model.Username,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                PasswordSalt = model.PasswordSalt,
                Role = "User",
                IsActive = true
            };

            const string insertUserSql = @"
                INSERT INTO users (id, created, updated, username, email, password_hash, password_salt, role, is_active)
                VALUES (@Id, @Created, @Updated, @Username, @Email, @PasswordHash, @PasswordSalt, @Role, @IsActive);";

            const string insertUserInfoSql = @"
                INSERT INTO user_info (id, user_id, created, updated, birthdate)
                VALUES (@Id, @UserId, @Created, @Updated, @Birthdate);";

            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }

            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                var commandDefUser = new CommandDefinition(insertUserSql, user, transaction, cancellationToken: cancellationToken);
                await _dbConnection.ExecuteAsync(commandDefUser);

                var userInfoParams = new UserInfo
                {
                    Id = userInfoId,
                    UserId = userId,
                    Created = now
                };

                var commandDefInfo = new CommandDefinition(insertUserInfoSql, userInfoParams, transaction, cancellationToken: cancellationToken);
                await _dbConnection.ExecuteAsync(commandDefInfo);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            return user.MapToModel();
        }
    }
}
