using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Application.Interfaces.Repositories;

public interface IVoucherRepository
{
    Task<CreateUploadFileResponse> CreateUploadFileAsync(CreateUploadFileRequest request);

    Task<CreateVoucherHeaderResponse> CreateVoucherHeaderAsync(CreateVoucherHeaderRequest request);

    Task<int> BulkInsertVoucherAsync(List<VoucherImportDto> vouchers);

    Task UpdateUploadFileResultAsync(
        int uploadFileId,
        int totalRecords,
        int successRecords,
        int failedRecords,
        string response,
        string updatedBy);

    Task MarkVoucherHeaderProcessedAsync(
        int voucherHeaderId,
        string updatedBy);
    Task<VoucherHeaderDto?> GetVoucherHeaderAsync(int voucherHeaderId);

    Task<List<string>> GetExistingVoucherCodesAsync(List<string> voucherCodes);

}