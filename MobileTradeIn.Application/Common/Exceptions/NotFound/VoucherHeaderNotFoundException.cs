using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.NotFound
{
    public class VoucherHeaderNotFoundException : BusinessException
    {
        public VoucherHeaderNotFoundException() : base("Voucher header not found.")
        { }
    }
}
