using MobileTradeIn.Api.Middlewares;

namespace MobileTradeIn.Api.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalException(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionMiddleware>();
    }
}