namespace MobileTradeIn.Api.Common.Responses;

public class ApiResponse<T>
{
    public bool Success { get; init; }

    public T? Data { get; init; }

    public string? Message { get; init; }

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    public string? TraceId { get; init; }
}