namespace ecommerce_app.backend.web.Entities
{
    public class WallPost : IAuditEntity
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public string Content { get; set; }
    }
}
