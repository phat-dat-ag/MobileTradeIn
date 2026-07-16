using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Application.Common.Exceptions.NotFound
{
    public class EmailTemplateNotFoundException : BusinessException
    {
        public EmailTemplateNotFoundException() : base("Email Template not found")
        {
        }
    }
}
