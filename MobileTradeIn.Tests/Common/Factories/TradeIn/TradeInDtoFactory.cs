using MobileTradeIn.Application.DTOs.TradeIn;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class TradeInDtoFactory
    {
        public static TradeInDto CreateTradeInDto()
        {
            return new TradeInDto
            {
                TradeInOfferId = 1,
                OfferAmount = 100000,
                IMEI = "1111111111",
                OfferDate = DateTime.Now,
                VoucherCode = "CODE",
                OriginalAmount = 90000,
                VoucherAmount = 10000,
            };
        }
    }
}
