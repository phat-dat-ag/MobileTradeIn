using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;
using System.Diagnostics;

namespace MobileTradeIn.Infrastructure.Repositories;

public class VoucherRepository : IVoucherRepository
{
    private readonly DapperContext _context;
    private readonly ILogger<VoucherRepository> _logger;

    public VoucherRepository(DapperContext context, ILogger<VoucherRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreateUploadFileResponse> CreateUploadFileAsync(
        CreateUploadFileRequest request)
    {
        const string storedProcedure = "trs.spUploadFile_Create";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@FileName", request.FileName);
        parameters.Add("@FilePath", request.FilePath);
        parameters.Add("@FileType", request.FileType);
        parameters.Add("@UploadedBy", request.UploadedBy);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. FileName={FileName}, FileType={FileType}",
            storedProcedure,
            request.FileName,
            request.FileType);

        var result =
            await connection.QueryFirstAsync<CreateUploadFileResponse>(
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

    public async Task<CreateVoucherHeaderResponse> CreateVoucherHeaderAsync(
        CreateVoucherHeaderRequest request)
    {
        const string storedProcedure = "mdm.spVoucherHeader_Create";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@UploadFileId", request.UploadFileId);
        parameters.Add("@VoucherBatchCode", request.VoucherBatchCode);
        parameters.Add("@ProductId", request.ProductId);
        parameters.Add("@VoucherValue", request.VoucherValue);
        parameters.Add("@Quantity", request.Quantity);
        parameters.Add("@Description", request.Description);
        parameters.Add("@CreatedBy", request.CreatedBy);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. UploadFileId={UploadFileId}, ProductId={ProductId}, Quantity={Quantity}",
            storedProcedure,
            request.UploadFileId,
            request.ProductId,
            request.Quantity);


        var result = await connection.QueryFirstAsync<CreateVoucherHeaderResponse>(
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

    public async Task<int> BulkInsertVoucherAsync(List<VoucherImportDto> vouchers)
    {
        const string storedProcedure = "mdm.spVoucher_BulkInsert";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var table = new DataTable();

        table.Columns.Add("VoucherCode", typeof(string));
        table.Columns.Add("VoucherHeaderId", typeof(int));
        table.Columns.Add("StartDate", typeof(DateTime));
        table.Columns.Add("EndDate", typeof(DateTime));
        table.Columns.Add("IsActive", typeof(bool));
        table.Columns.Add("CreatedBy", typeof(string));

        foreach (var voucher in vouchers)
        {
            table.Rows.Add(
                voucher.VoucherCode,
                voucher.VoucherHeaderId,
                voucher.StartDate.ToDateTime(TimeOnly.MinValue),
                voucher.EndDate.ToDateTime(TimeOnly.MinValue),
                voucher.IsActive,
                voucher.CreatedBy);
        }

        var parameters = new DynamicParameters();

        parameters.Add(
            "@VoucherList",
            table.AsTableValuedParameter("mdm.VoucherImportType"));

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. VoucherCount={VoucherCount}",
            storedProcedure,
            vouchers.Count);

        var result = await connection.QueryFirstAsync<BulkInsertResponse>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. TotalInserted={TotalInserted}",
            storedProcedure,
            stopwatch.ElapsedMilliseconds,
            result.TotalInserted);

        return result.TotalInserted;
    }

    public async Task UpdateUploadFileResultAsync(
        int uploadFileId,
        int totalRecords,
        int successRecords,
        int failedRecords,
        string response,
        string updatedBy)
    {
        const string storedProcedure = "trs.spUploadFile_UpdateResult";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@UploadFileId", uploadFileId);
        parameters.Add("@TotalRecords", totalRecords);
        parameters.Add("@SuccessRecords", successRecords);
        parameters.Add("@FailedRecords", failedRecords);
        parameters.Add("@Response", response);
        parameters.Add("@UpdatedBy", updatedBy);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. UploadFileId={UploadFileId}, TotalRecords={TotalRecords}, SuccessRecords={SuccessRecords}, FailedRecords={FailedRecords}",
            storedProcedure,
            uploadFileId,
            totalRecords,
            successRecords,
            failedRecords);

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

    public async Task MarkVoucherHeaderProcessedAsync(int voucherHeaderId, string updatedBy)
    {
        const string storedProcedure = "mdm.spVoucherHeader_MarkProcessed";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var parameters = new DynamicParameters();

        parameters.Add("@VoucherHeaderId", voucherHeaderId);
        parameters.Add("@UpdatedBy", updatedBy);

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. VoucherHeaderId={VoucherHeaderId}",
            storedProcedure,
            voucherHeaderId);

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

    public async Task<VoucherHeaderDto?> GetVoucherHeaderAsync(int voucherHeaderId)
    {
        const string storedProcedure = "mdm.spVoucherHeader_GetById";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. VoucherHeaderId={VoucherHeaderId}",
            storedProcedure,
            voucherHeaderId);

        var result = await connection.QueryFirstOrDefaultAsync<VoucherHeaderDto>(
            storedProcedure,
            new
            {
                VoucherHeaderId = voucherHeaderId
            },
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. VoucherHeaderFound={VoucherHeaderFound}",
            storedProcedure,
            stopwatch.ElapsedMilliseconds,
            result is not null);

        return result;
    }

    public async Task<List<string>> GetExistingVoucherCodesAsync(List<string> voucherCodes)
    {
        const string storedProcedure = "mdm.spVoucher_GetExistingCodes";

        using var connection = _context.CreateConnection();

        var stopwatch = Stopwatch.StartNew();

        var table = new DataTable();
        table.Columns.Add("VoucherCode", typeof(string));

        foreach (var code in voucherCodes)
        {
            table.Rows.Add(code);
        }

        var parameters = new DynamicParameters();

        parameters.Add(
            "@VoucherCodes",
            table.AsTableValuedParameter("mdm.VoucherCodeList"));

        _logger.LogInformation(
            "Executing stored procedure {StoredProcedure}. VoucherCodeCount={VoucherCodeCount}",
            storedProcedure,
            voucherCodes.Count);

        var result = await connection.QueryAsync<string>(
            storedProcedure,
            parameters,
            commandType: CommandType.StoredProcedure);

        stopwatch.Stop();

        var existingVoucherCodes = result.ToList();

        _logger.LogInformation(
            "Stored procedure {StoredProcedure} completed in {ElapsedMilliseconds} ms. ExistingVoucherCount={ExistingVoucherCount}",
            storedProcedure,
            stopwatch.ElapsedMilliseconds,
            existingVoucherCodes.Count);

        return existingVoucherCodes;
    }
}