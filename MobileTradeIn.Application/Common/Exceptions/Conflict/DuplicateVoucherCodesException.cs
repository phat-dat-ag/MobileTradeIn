using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Conflict
{
    public class DuplicateVoucherCodesException : BusinessException
    {
        public DuplicateVoucherCodesException(string codes)
            : base($"Duplicate voucher codes found in CSV: {codes}")
        { }
    }
}
