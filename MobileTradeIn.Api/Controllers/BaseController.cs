using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Api.Common.Responses;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult Success<T>(T data, string? message = null)
    {
        return Ok(new ApiResponse<T>
        {
            Success = true,
            Message = message,
            TraceId = HttpContext.TraceIdentifier,
            Data = data
        });
    }

    protected IActionResult Success(string? message = null)
    {
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = message,
            TraceId = HttpContext.TraceIdentifier,
            Data = null
        });
    }
}