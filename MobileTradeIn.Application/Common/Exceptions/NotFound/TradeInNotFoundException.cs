using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.NotFound
{
    public class TradeInNotFoundException : BusinessException
    {
        public TradeInNotFoundException(int TradeInOfferId)
            : base($"TradeIn not found with TradeInOfferId = {TradeInOfferId}")
        {
        }
    }
}
