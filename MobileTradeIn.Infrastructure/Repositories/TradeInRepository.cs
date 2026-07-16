using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;

namespace MobileTradeIn.Infrastructure.Repositories;

public class TradeInRepository : ITradeInRepository
{
    private readonly DapperContext _context;
    private readonly ILogger<TradeInRepository> _logger;

    public TradeInRepository(DapperContext context, ILogger<TradeInRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateTradeInResponse> CreateTradeInAsync(CreateTradeInRequest request)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", request.CustomerId);
        parameters.Add("@ProductId", request.ProductId);
        parameters.Add("@DeviceCondition", request.DeviceCondition);
        parameters.Add("@IMEI", request.IMEI);
        parameters.Add("@VoucherCode", request.VoucherCode);
        parameters.Add("@CreatedBy", request.CreatedBy);

        _logger.LogInformation("Executing stored procedure trs.spTradeIn_Create");

        var result = await connection.QueryFirstAsync<CreateTradeInResponse>(
            "trs.spTradeIn_Create",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure trs.spTradeIn_Create executed successfully.");

        return result;
    }

    public async Task ConfirmTradeInAsync(ConfirmTradeInRequest request)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@TradeInOfferId", request.TradeInOfferId);
        parameters.Add("@ConfirmedBy", request.ConfirmedBy);
        parameters.Add("@Notes", request.Notes);

        _logger.LogInformation("Executing stored procedure trs.spTradeIn_Confirm");

        await connection.ExecuteAsync(
            "trs.spTradeIn_Confirm",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure trs.spTradeIn_Confirm executed successfully.");
    }

    public async Task RejectTradeInAsync(RejectTradeInRequest request)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@TradeInOfferId", request.TradeInOfferId);
        parameters.Add("@RejectedBy", request.RejectedBy);
        parameters.Add("@Notes", request.Notes);

        _logger.LogInformation("Executing stored procedure trs.spTradeIn_Reject");

        await connection.ExecuteAsync(
            "trs.spTradeIn_Reject",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure trs.spTradeIn_Reject executed successfully.");
    }

    public async Task<TradeInDto?> GetTradeInByIdAsync(int tradeInOfferId)
    {
        using var connection = _context.CreateConnection();

        _logger.LogInformation("Executing stored procedure trs.spTradeIn_GetById");

        var result = await connection.QueryFirstOrDefaultAsync<TradeInDto>(
            "trs.spTradeIn_GetById",
            new
            { tradeInOfferId = tradeInOfferId },
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure trs.spTradeIn_GetById executed successfully.");

        return result;
    }

    public async Task<TradeInEmailDto?> GetTradeInEmailAsync(int tradeInOfferId)
    {
        using var connection = _context.CreateConnection();

        _logger.LogInformation("Executing stored procedure trs.spTradeIn_GetEmail");

        var result = await connection.QueryFirstOrDefaultAsync<TradeInEmailDto>(
                "trs.spTradeIn_GetEmail",
                new
                {
                    TradeInOfferId = tradeInOfferId
                },
               commandType: CommandType.StoredProcedure);

        _logger.LogInformation(
            "Stored procedure trs.spTradeIn_GetEmail executed successfully.");

        return result;
    }
}
