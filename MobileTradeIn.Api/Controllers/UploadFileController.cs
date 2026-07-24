using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/upload-files")]
public class UploadFileController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<UploadFileController> _logger;

    public UploadFileController(IMediator mediator, ILogger<UploadFileController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUploadFileCommand command)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}",
            "CreateUploadFile");

        var result = await _mediator.Send(command);

        _logger.LogInformation(
            "API Completed. Operation={Operation}",
            "CreateUploadFile");

        return Success(result, "Upload file created successfully.");
    }
}