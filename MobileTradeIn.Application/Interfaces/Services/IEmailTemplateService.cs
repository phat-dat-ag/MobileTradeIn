namespace MobileTradeIn.Application.Interfaces.Services;

public interface IEmailTemplateService
{
    string RenderContentFromEmailTemplate(string template, Dictionary<string, string> values);
}