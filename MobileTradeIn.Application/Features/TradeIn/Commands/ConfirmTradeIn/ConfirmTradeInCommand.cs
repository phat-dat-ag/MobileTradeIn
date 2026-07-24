using MediatR;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInCommand : IRequest
{
    public int TradeInOfferId { get; set; }

    public string ConfirmedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}