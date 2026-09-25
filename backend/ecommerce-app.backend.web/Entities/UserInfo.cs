namespace ecommerce_app.backend.web.Entities
{
    public class UserInfo : IAuditEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? City { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
    }
}
