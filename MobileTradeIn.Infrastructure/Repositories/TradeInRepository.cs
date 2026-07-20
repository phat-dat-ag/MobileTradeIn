using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;
using System.Diagnostics;

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
        const string storedProcedure = "trs.spTradeIn_Create";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@CustomerId", request.CustomerId);
        parameters.Add("@ProductId", request.ProductId);
        parameters.Add("@DeviceCondition", request.DeviceCondition);
        parameters.Add("@IMEI", request.IMEI);
        parameters.Add("@VoucherCode", request.VoucherCode);
        parameters.Add("@CreatedBy", request.CreatedBy);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. CustomerId={CustomerId}, ProductId={ProductId}",
            storedProcedure,
            request.CustomerId,
            request.ProductId);

        var result = await connection.QueryFirstAsync<CreateTradeInResponse>(
           storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms.",
            storedProcedure,
            stopwatch.ElapsedMilliseconds);

        return result;
    }

    public async Task ConfirmTradeInAsync(ConfirmTradeInRequest request)
    {
        const string storedProcedure = "trs.spTradeIn_Confirm";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@TradeInOfferId", request.TradeInOfferId);
        parameters.Add("@ConfirmedBy", request.ConfirmedBy);
        parameters.Add("@Notes", request.Notes);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. TradeInOfferId={TradeInOfferId}",
            storedProcedure,
            request.TradeInOfferId);

        await connection.ExecuteAsync(
           storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms.",
            storedProcedure,
            stopwatch.ElapsedMilliseconds);
    }

    public async Task RejectTradeInAsync(RejectTradeInRequest request)
    {
        const string storedProcedure = "trs.spTradeIn_Reject";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@TradeInOfferId", request.TradeInOfferId);
        parameters.Add("@RejectedBy", request.RejectedBy);
        parameters.Add("@Notes", request.Notes);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. TradeInOfferId={TradeInOfferId}",
            storedProcedure,
            request.TradeInOfferId);

        await connection.ExecuteAsync(
           storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms.",
            storedProcedure,
            stopwatch.ElapsedMilliseconds);
    }

    public async Task<TradeInDto?> GetTradeInByIdAsync(int tradeInOfferId)
    {
        const string storedProcedure = "trs.spTradeIn_GetById";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. TradeInOfferId={TradeInOfferId}",
            storedProcedure,
            tradeInOfferId);


        var result = await connection.QueryFirstOrDefaultAsync<TradeInDto>(
            storedProcedure,
            new
            { tradeInOfferId = tradeInOfferId },
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. TradeInFound={TradeInFound}",
            storedProcedure,
            stopwatch.ElapsedMilliseconds,
            result is not null);


        return result;
    }

    public async Task<TradeInEmailDto?> GetTradeInEmailAsync(int tradeInOfferId)
    {
        const string storedProcedure = "trs.spTradeIn_GetEmail";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. TradeInOfferId={TradeInOfferId}",
            storedProcedure,
            tradeInOfferId);

        var result = await connection.QueryFirstOrDefaultAsync<TradeInEmailDto>(
                storedProcedure,
                new
                {
                    TradeInOfferId = tradeInOfferId
                },
               commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. EmailDataFound={EmailDataFound}",
            storedProcedure,
            stopwatch.ElapsedMilliseconds,
            result is not null);

        return result;
    }
}
