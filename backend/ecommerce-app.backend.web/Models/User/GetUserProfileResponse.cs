namespace ecommerce_app.backend.web.Models.User
{
    public class GetUserProfileResponse
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? City { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}
