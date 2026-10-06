namespace ecommerce_app.backend.web.Entities
{
    public class EmailConfirmationToken : IEntity
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public Guid UserId { get; set; }
        public string TokenHash { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
