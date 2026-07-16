using MobileTradeIn.Application.DTOs.Email;

namespace MobileTradeIn.Application.Interfaces.Repositories;

public interface IEmailTemplateRepository
{
    Task<EmailTemplateDto?> GetEmailTemplateByTemplateCodeAsync(string templateCode);
}