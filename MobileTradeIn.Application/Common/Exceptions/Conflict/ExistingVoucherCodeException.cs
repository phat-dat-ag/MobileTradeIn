using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Conflict
{
    public class ExistingVoucherCodeException : BusinessException
    {
        public ExistingVoucherCodeException(string existingCodes)
            : base($"Voucher codes already exist: {existingCodes}")
        { }
    }
}
