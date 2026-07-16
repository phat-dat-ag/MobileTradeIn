using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.Validation;

public class InvalidDeviceConditionException : BusinessException
{
    public InvalidDeviceConditionException(string condition)
        : base($"Device condition '{condition}' is invalid.")
    {
    }
}