using MobileTradeIn.Application.Interfaces.Services;

namespace MobileTradeIn.Infrastructure.Services;

public class EmailTemplateService : IEmailTemplateService
{
    public string RenderContentFromEmailTemplate(string template, Dictionary<string, string> values)
    {
        foreach (var item in values)
        {
            template = template.Replace(
                $"{{{{{item.Key}}}}}",
                item.Value);
        }

        return template;
    }
}