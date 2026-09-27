using ecommerce_app.backend.web.Common.Providers;

namespace ecommerce_app.backend.web.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
            => services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }
}
