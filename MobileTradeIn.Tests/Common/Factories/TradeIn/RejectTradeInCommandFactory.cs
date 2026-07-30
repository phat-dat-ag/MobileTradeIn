using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class RejectTradeInCommandFactory
    {
        public static RejectTradeInCommand CreateRejectTradeInCommand(int tradeInOfferId, string rejectedBy, string notes)
        {
            return new RejectTradeInCommand
            {
                TradeInOfferId = tradeInOfferId,
                RejectedBy = rejectedBy,
                Notes = notes
            };
        }
    }
}
