namespace MobileTradeIn.Application.DTOs.Voucher;

public class CreateVoucherHeaderResponse
{
    public int VoucherHeaderId { get; set; }

    public string VoucherBatchCode { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}