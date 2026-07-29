using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Application.Interfaces.Repositories;

public interface IVoucherRepository
{
    Task<CreateVoucherHeaderResponse> CreateVoucherHeaderAsync(CreateVoucherHeaderRequest request);

    Task<int> BulkInsertVoucherAsync(List<VoucherImportDto> vouchers);

    Task MarkVoucherHeaderProcessedAsync(
        int voucherHeaderId,
        string updatedBy);
    Task<VoucherHeaderDto?> GetVoucherHeaderAsync(int voucherHeaderId);

    Task<List<string>> GetExistingVoucherCodesAsync(List<string> voucherCodes);

}