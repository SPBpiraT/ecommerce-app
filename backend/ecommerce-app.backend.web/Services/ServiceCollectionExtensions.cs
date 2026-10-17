using ecommerce_app.backend.web.Services.Auth;
using ecommerce_app.backend.web.Services.Email;
using ecommerce_app.backend.web.Services.Order;
using ecommerce_app.backend.web.Services.User;

namespace ecommerce_app.backend.web.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
