namespace ecommerce_app.backend.web.Entities
{
    public interface IAuditEntity : IEntity
    {
        DateTime? Updated { get; set; }
    }
}
