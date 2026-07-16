using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/tradein")]
public class TradeInController : ControllerBase
{
    private readonly IMediator _mediator;

    public TradeInController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTradeIn(CreateTradeInCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPatch("confirm")]
    public async Task<IActionResult> ConfirmTradeInRequest([FromBody] ConfirmTradeInCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            Message = "Trade-in request confirmed successfully."
        });
    }

    [HttpPatch("reject")]
    public async Task<IActionResult> RejectTradeInRequest([FromBody] RejectTradeInCommand command)
    {
        await _mediator.Send(command);

        return Ok(new
        {
            Message = "Trade-in request rejected successfully."
        });
    }

    [HttpGet("{TradeInOfferId}")]
    public async Task<IActionResult> GetTradeInRequestById(int TradeInOfferId)
    {
        var result = await _mediator.Send(
            new GetTradeInByIdQuery
            {
                TradeInOfferId = TradeInOfferId
            });

        return Ok(result);
    }
}