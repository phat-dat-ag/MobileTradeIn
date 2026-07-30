using MobileTradeIn.Application.DTOs.Email;

namespace MobileTradeIn.Tests.Common.Factories.Email
{
    public class TradeInEmailDtoFactory
    {
        public static TradeInEmailDto CreateTradeInEmailDto()
        {
            return new TradeInEmailDto
            {
                CustomerName = "Nguyen Van A",
                CustomerEmail = "test@gmail.com",
                ProductName = "iPhone 15",
                OfferAmount = 10000000,
                TransactionNumber = "TRX0001"
            };
        }
    }
}
