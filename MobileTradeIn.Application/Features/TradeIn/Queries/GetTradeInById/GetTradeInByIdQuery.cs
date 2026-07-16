using MediatR;
using MobileTradeIn.Application.DTOs.TradeIn;

namespace MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

public class GetTradeInByIdQuery : IRequest<TradeInDto?>
{
    public int TradeInOfferId { get; set; }
}