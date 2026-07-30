using MobileTradeIn.Application.DTOs.Email;

namespace MobileTradeIn.Tests.Common.Factories.Email
{
    public class EmailTemplateDtoFactory
    {
        public static EmailTemplateDto CreateEmailTemplateDto(string subject, string content)
        {
            return new EmailTemplateDto
            {
                Subject = subject,
                Content = content
            };
        }
    }
}
