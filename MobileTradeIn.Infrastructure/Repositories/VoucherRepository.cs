using Dapper;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Infrastructure.Persistence;
using System.Data;

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
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@FileName", request.FileName);
        parameters.Add("@FilePath", request.FilePath);
        parameters.Add("@FileType", request.FileType);
        parameters.Add("@UploadedBy", request.UploadedBy);

        _logger.LogInformation("Executing stored procedure trs.spUploadFile_Create");

        var result =
            await connection.QueryFirstAsync<CreateUploadFileResponse>(
                "trs.spUploadFile_Create",
                parameters,
                commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure trs.spUploadFile_Create executed successfully.");

        return result;
    }

    public async Task<CreateVoucherHeaderResponse> CreateVoucherHeaderAsync(
        CreateVoucherHeaderRequest request)
    {
        using var connection = _context.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@UploadFileId", request.UploadFileId);
        parameters.Add("@VoucherBatchCode", request.VoucherBatchCode);
        parameters.Add("@ProductId", request.ProductId);
        parameters.Add("@VoucherValue", request.VoucherValue);
        parameters.Add("@Quantity", request.Quantity);
        parameters.Add("@Description", request.Description);
        parameters.Add("@CreatedBy", request.CreatedBy);

        _logger.LogInformation("Executing stored procedure mdm.spVoucherHeader_Create");

        var result = await connection.QueryFirstAsync<CreateVoucherHeaderResponse>(
            "mdm.spVoucherHeader_Create",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure mdm.spVoucherHeader_Create executed successfully.");

        return result;
    }

    public async Task<int> BulkInsertVoucherAsync(List<VoucherImportDto> vouchers)
    {
        using var connection = _context.CreateConnection();

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

        _logger.LogInformation("Executing stored procedure mdm.spVoucher_BulkInsert");

        var result = await connection.QueryFirstAsync<BulkInsertResponse>(
            "mdm.spVoucher_BulkInsert",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("Stored procedure mdm.spVoucher_BulkInsert executed successfully.");

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
        using var connection = _context.CreateConnection();

        _logger.LogInformation(
            "Executing stored procedure trs.spUploadFile_UpdateResult");

        var parameters = new DynamicParameters();

        parameters.Add("@UploadFileId", uploadFileId);
        parameters.Add("@TotalRecords", totalRecords);
        parameters.Add("@SuccessRecords", successRecords);
        parameters.Add("@FailedRecords", failedRecords);
        parameters.Add("@Response", response);
        parameters.Add("@UpdatedBy", updatedBy);

        await connection.ExecuteAsync(
            "trs.spUploadFile_UpdateResult",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation(
            "Stored procedure trs.spUploadFile_UpdateResult executed successfully.");
    }

    public async Task MarkVoucherHeaderProcessedAsync(int voucherHeaderId, string updatedBy)
    {
        using var connection = _context.CreateConnection();

        _logger.LogInformation(
            "Executing stored procedure mdm.spVoucherHeader_MarkProcessed");

        var parameters = new DynamicParameters();

        parameters.Add("@VoucherHeaderId", voucherHeaderId);
        parameters.Add("@UpdatedBy", updatedBy);

        await connection.ExecuteAsync(
            "mdm.spVoucherHeader_MarkProcessed",
            parameters,
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation(
            "Stored procedure mdm.spVoucherHeader_MarkProcessed executed successfully.");
    }

    public async Task<VoucherHeaderDto?> GetVoucherHeaderAsync(int voucherHeaderId)
    {
        using var connection = _context.CreateConnection();

        const string sql =
            """
                SELECT
                    VoucherHeaderId,
                    Quantity
                FROM mdm.VoucherHeader
                WHERE VoucherHeaderId=@VoucherHeaderId
                    AND IsDeleted=0;
            """;

        _logger.LogInformation("Executing querying voucher header voucherHeaderId = {voucherHeaderId}.", voucherHeaderId);

        var result = await connection.QueryFirstOrDefaultAsync<VoucherHeaderDto>(
            sql,
            new
            {
                VoucherHeaderId = voucherHeaderId
            });

        _logger.LogInformation("Executing querying voucher header voucherHeaderId = {voucherHeaderId} successfully.", voucherHeaderId);

        return result;
    }

    public async Task<List<string>> GetExistingVoucherCodesAsync(List<string> voucherCodes)
    {
        using var connection = _context.CreateConnection();

        const string sql = """
                                SELECT VoucherCode
                                FROM mdm.VoucherDetail
                                WHERE VoucherCode IN @VoucherCodes
                                  AND IsDeleted = 0;
                            """;

        var result = await connection.QueryAsync<string>(
            sql,
            new
            {
                VoucherCodes = voucherCodes
            });

        return result.ToList();
    }
}