using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Application.Interfaces.Services;

public interface ICsvService
{
    Task<List<VoucherImportDto>> ReadVoucherCsvAsync(
        Stream stream,
        int voucherHeaderId,
        string createdBy);
}