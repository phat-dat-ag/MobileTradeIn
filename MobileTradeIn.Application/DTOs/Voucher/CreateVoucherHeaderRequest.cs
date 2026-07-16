namespace MobileTradeIn.Application.DTOs.Voucher;

public class CreateVoucherHeaderRequest
{
    public int UploadFileId { get; set; }

    public string VoucherBatchCode { get; set; } = string.Empty;

    public int ProductId { get; set; }

    public decimal VoucherValue { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}