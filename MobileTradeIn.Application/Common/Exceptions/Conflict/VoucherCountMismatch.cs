using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Conflict
{
    public class VoucherCountMismatch : BusinessException
    {
        public VoucherCountMismatch(int quantity, int actualQuantity)
            : base($"Expected {quantity} vouchers but found {actualQuantity}.")
        { }
    }
}
