namespace ecommerce_app.backend.web.Entities
{
    public class User : IAuditEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailConfirmed { get; set; }
    }
}
