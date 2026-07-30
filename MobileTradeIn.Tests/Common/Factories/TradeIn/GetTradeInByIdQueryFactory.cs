using MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class GetTradeInByIdQueryFactory
    {
        public static GetTradeInByIdQuery CreateGetTradeInByIdQuery(int tradeInOfferId)
        {
            return new GetTradeInByIdQuery
            {
                TradeInOfferId = tradeInOfferId,
            };
        }
    }
}
