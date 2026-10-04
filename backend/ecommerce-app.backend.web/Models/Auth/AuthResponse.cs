namespace ecommerce_app.backend.web.Models.Auth
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
