using ecommerce_app.backend.web.Exceptions;

namespace ecommerce_app.backend.web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AccessDeniedException ade)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(new { Message = "Access denied" });
            }
            catch (AuthException aex)
            {
                _logger.LogError(aex, "Auth exception");

                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { Message = aex.Message, StackTrace = aex.StackTrace });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured");

                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}
