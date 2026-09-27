namespace ecommerce_app.backend.web.Common.Providers
{
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
