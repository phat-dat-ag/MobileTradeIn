using MediatR;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInCommand : IRequest
{
    public int TradeInOfferId { get; set; }

    public string RejectedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}