using ecommerce_app.backend.web.Entities;
using ecommerce_app.backend.web.Models.User;

namespace ecommerce_app.backend.web.Mapping
{
    public static class UserMappingExtensions
    {
        public static UserModel MapToModel(this User user)
            => new()
            {
                Id = user.Id,
                Created = user.Created,
                Updated = user.Updated,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                PasswordSalt = user.PasswordSalt,
                Role = user.Role
            };
    }
}
