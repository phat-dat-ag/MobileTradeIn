using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Validation;

public class InvalidVoucherCodeException : BusinessException
{
    public InvalidVoucherCodeException()
        : base("VoucherCode is invalid.")
    {
    }
}