using ecommerce_app.backend.web.Mapping;
using ecommerce_app.backend.web.Models.User;
using ecommerce_app.backend.web.Entities;
using ecommerce_app.backend.web.Common.Providers;

using System.Data;
using Dapper;

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

        public async Task<UserModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT 
                    id, 
                    created, 
                    updated, 
                    username, 
                    email, 
                    password_hash AS passwordhash, 
                    password_salt AS passwordsalt, 
                    role,
                    is_active AS isactive,
                    is_email_confirmed AS isemailconfirmed
               FROM users 
               WHERE id = @id 
                AND is_active = true";

            return await _dbConnection.QueryFirstOrDefaultAsync<UserModel>(new CommandDefinition(
                sql,
                new { Id = id },
                cancellationToken: cancellationToken));
        }

        public async Task<UserModel?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT 
                    id, 
                    created, 
                    updated, 
                    username, 
                    email, 
                    password_hash AS passwordhash, 
                    password_salt AS passwordsalt, 
                    role,
                    is_active AS isactive,
                    is_email_confirmed AS isemailconfirmed
               FROM users 
               WHERE username = @Username 
                AND is_active = true";

            return await _dbConnection.QueryFirstOrDefaultAsync<UserModel>(new CommandDefinition(
                sql,
                new { Username = username },
                cancellationToken: cancellationToken));
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

        public async Task SaveConfirmationTokenAsync(EmailConfirmationToken token, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                INSERT INTO email_confirmation_tokens (id, user_id, token_hash, expires_at)
                VALUES (@Id, @UserId, @TokenHash, @ExpiresAt);";

            var command = new CommandDefinition(sql, token, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }

        public async Task<EmailConfirmationToken?> GetTokenInfoAsync(string tokenHash, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                SELECT 
                    id, 
                    user_id AS userid, 
                    token_hash AS tokenhash, 
                    expires_at AS expiresat 
                FROM email_confirmation_tokens 
                WHERE token_hash = @TokenHash";

            var command = new CommandDefinition(sql, new { TokenHash = tokenHash }, cancellationToken: cancellationToken);
            return await _dbConnection.QueryFirstOrDefaultAsync<EmailConfirmationToken>(command);
        }

        public async Task UpdateUserAsync(UserModel model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmEmailStatusAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE users 
                SET is_email_confirmed = true,
                    updated = @Updated
                WHERE id = @Id";

            var parameters = new
            {
                Id = userId,
                Updated = _dateTimeProvider.UtcNow
            };

            var command = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }

        public async Task DeleteConfirmationTokenAsync(Guid tokenId, CancellationToken cancellationToken = default)
        {
            const string sql = "DELETE FROM email_confirmation_tokens WHERE id = @Id";

            var command = new CommandDefinition(sql, new { Id = tokenId }, cancellationToken: cancellationToken);
            await _dbConnection.ExecuteAsync(command);
        }
    }
}
