using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;

namespace MobileTradeIn.Infrastructure.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly DapperContext _context;
    private readonly ILogger<EmailTemplateRepository> _logger;

    public EmailTemplateRepository(DapperContext context, ILogger<EmailTemplateRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EmailTemplateDto?> GetEmailTemplateByTemplateCodeAsync(string templateCode)
    {
        using var connection = _context.CreateConnection();

        _logger.LogInformation(
            "Getting EmailTemplate. TemplateCode={TemplateCode}",
            templateCode);

        var result = await connection.QueryFirstOrDefaultAsync<EmailTemplateDto>(
            "cnf.spEmailTemplate_GetByTemplateCode",
            new
            {
                TemplateCode = templateCode
            },
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation(
            "EmailTemplate query completed.");

        return result;
    }
}