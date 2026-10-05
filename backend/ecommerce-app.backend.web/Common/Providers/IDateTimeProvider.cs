namespace ecommerce_app.backend.web.Common.Providers
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}
