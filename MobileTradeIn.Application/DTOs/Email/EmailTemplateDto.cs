namespace MobileTradeIn.Application.DTOs.Email;

public class EmailTemplateDto
{
    public string TemplateCode { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}