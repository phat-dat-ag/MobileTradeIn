using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;
using System.Diagnostics;

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
        const string storedProcedure = "cnf.spEmailTemplate_GetByTemplateCode";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
           "Executing stored procedure {StoredProcedure} with TemplateCode={TemplateCode}",
           storedProcedure,
           templateCode);

        var result = await connection.QueryFirstOrDefaultAsync<EmailTemplateDto>(
            storedProcedure,
            new
            {
                TemplateCode = templateCode
            },
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
           "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. TemplateFound={TemplateFound}",
           storedProcedure,
           stopwatch.ElapsedMilliseconds,
           result is not null);

        return result;
    }
}