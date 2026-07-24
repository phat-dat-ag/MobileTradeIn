using MobileTradeIn.Api.Middlewares;

namespace MobileTradeIn.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCorrelationId(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}