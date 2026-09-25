namespace ecommerce_app.backend.web.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }

        DateTime Created { get; set; }
    }
}
