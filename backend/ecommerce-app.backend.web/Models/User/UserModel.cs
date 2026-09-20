namespace ecommerce_app.backend.web.Models.User
{
    public class UserModel
    {
        public Guid Id { get; init; }

        public DateTime Created { get; init; }

        public string Username { get; init; }

        public string PasswordHash { get; init; }

        public string PasswordSalt { get; init; }
    }
}
