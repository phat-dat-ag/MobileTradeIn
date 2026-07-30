using MobileTradeIn.Application.DTOs.TradeIn;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class CreateTradeInResponseFactory
    {
        public static CreateTradeInResponse CreateCreateTradeInResponse()
        {
            return new CreateTradeInResponse
            {
                TradeInRequestId = 1,
                TradeInOfferId = 1,
                OfferAmount = 10000000
            };
        }
    }
}
