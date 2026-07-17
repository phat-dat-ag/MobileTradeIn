using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Validation
{
    public class NoVoucherException : BusinessException
    {
        public NoVoucherException() : base("CSV contains no voucher.")
        { }
    }
}
