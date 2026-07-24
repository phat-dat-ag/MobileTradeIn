using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Application.Interfaces.Services;

public interface IFileReader
{
    bool CanRead(string extension);

    List<VoucherImportDto> ReadAsync(
        Stream stream,
        int voucherHeaderId,
        string createdBy);
}