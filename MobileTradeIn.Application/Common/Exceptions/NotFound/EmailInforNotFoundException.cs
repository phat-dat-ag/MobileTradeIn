using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.NotFound
{
    public class EmailInforNotFoundException : BusinessException
    {
        public EmailInforNotFoundException() : base("Email Infor not found")
        {
        }
    }
}
