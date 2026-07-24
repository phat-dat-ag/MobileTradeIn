using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;
using MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/tradein")]
public class TradeInController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<TradeInController> _logger;

    public TradeInController(IMediator mediator, ILogger<TradeInController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTradeIn(CreateTradeInCommand command)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}",
            "CreateTradeIn");

        var result = await _mediator.Send(command);

        _logger.LogInformation(
            "API Completed. Operation={Operation}",
            "CreateTradeIn");

        return Success(result, "Trade-in request created successfully.");
    }

    [HttpPatch("confirm")]
    public async Task<IActionResult> ConfirmTradeInRequest([FromBody] ConfirmTradeInCommand command)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "ConfirmTradeIn",
            command.TradeInOfferId);

        await _mediator.Send(command);

        _logger.LogInformation(
            "API Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "ConfirmTradeIn",
            command.TradeInOfferId);

        return Success("Trade-in request confirmed successfully.");
    }

    [HttpPatch("reject")]
    public async Task<IActionResult> RejectTradeInRequest([FromBody] RejectTradeInCommand command)
    {
        _logger.LogInformation(
           "API Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
           "RejectTradeIn",
           command.TradeInOfferId);

        await _mediator.Send(command);

        _logger.LogInformation(
            "API Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "RejectTradeIn",
            command.TradeInOfferId);

        return Success("Trade-in request rejected successfully.");
    }

    [HttpGet("{TradeInOfferId}")]
    public async Task<IActionResult> GetTradeInRequestById(int TradeInOfferId)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "GetTradeIn",
            TradeInOfferId);

        var result = await _mediator.Send(
            new GetTradeInByIdQuery
            {
                TradeInOfferId = TradeInOfferId
            });

        _logger.LogInformation(
            "API Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "GetTradeIn",
            TradeInOfferId);

        return Success(result, "Trade-in retrieved successfully.");
    }
}