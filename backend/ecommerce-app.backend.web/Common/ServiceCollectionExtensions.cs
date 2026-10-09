using ecommerce_app.backend.web.Common.Configuration;
using ecommerce_app.backend.web.Common.Providers;
using Microsoft.Extensions.Configuration;

namespace ecommerce_app.backend.web.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.Configure<YooKassaSettings>(configuration.GetSection("YooKassaSettings"));

            return services;
        } 
    }
}
