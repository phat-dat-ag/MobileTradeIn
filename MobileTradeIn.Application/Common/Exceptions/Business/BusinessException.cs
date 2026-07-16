namespace MobileTradeIn.Application.Common.Exceptions.Business;

public class BusinessException : Exception
{
    public BusinessException(string message)
        : base(message)
    {
    }
}