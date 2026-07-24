using MobileTradeIn.Api.Constants;
using Serilog.Context;

namespace MobileTradeIn.Api.Middlewares;

public class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var value)
            ? value.ToString()
            : Guid.NewGuid().ToString();

        context.Response.Headers[HeaderName] = correlationId;

        context.Items[HttpContextKeys.CorrelationId] = correlationId;

        var userId = context.User?.Identity?.IsAuthenticated == true
            ? context.User.Identity!.Name
            : "Anonymous";

        context.Items[HttpContextKeys.UserId] = userId;

        using (LogContext.PushProperty("CorrelationId", correlationId))
        using (LogContext.PushProperty("UserId", userId))
        using (LogContext.PushProperty("RequestPath", context.Request.Path))
        using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        {
            await _next(context);
        }
    }
}