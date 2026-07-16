using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/upload-files")]
public class UploadFileController : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadFileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUploadFileCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}