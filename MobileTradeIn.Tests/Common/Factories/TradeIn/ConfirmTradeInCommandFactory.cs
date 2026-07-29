using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class ConfirmTradeInCommandFactory
    {
        public static ConfirmTradeInCommand CreateConfirmTradeInCommand(
            int tradeInOfferId, string confirmedBy, string notes)
        {
            return new ConfirmTradeInCommand
            {
                TradeInOfferId = tradeInOfferId,
                ConfirmedBy = confirmedBy,
                Notes = notes
            };
        }
    }
}
