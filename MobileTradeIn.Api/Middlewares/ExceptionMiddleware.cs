using Microsoft.Data.SqlClient;
using MobileTradeIn.Api.Common.Responses;
using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. Path={Path}",
                context.Request.Path);

            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode = StatusCodes.Status500InternalServerError;
        string message = "Internal server error.";

        switch (exception)
        {
            case BusinessException businessEx:

                statusCode = StatusCodes.Status404NotFound;
                message = businessEx.Message;
                break;

            case SqlException sqlEx when sqlEx.Number is 50004 or 50005:

                statusCode = StatusCodes.Status409Conflict;
                message = sqlEx.Message;
                break;

            case SqlException sqlEx:

                statusCode = StatusCodes.Status400BadRequest;
                message = sqlEx.Message;
                break;

            case ArgumentException argEx:

                statusCode = StatusCodes.Status400BadRequest;
                message = argEx.Message;
                break;

            case KeyNotFoundException notFound:

                statusCode = StatusCodes.Status404NotFound;
                message = notFound.Message;
                break;

            default:

                message = "An unexpected error occurred.";
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = message,
            Data = null,
            TraceId = context.TraceIdentifier
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}