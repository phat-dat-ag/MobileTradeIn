namespace MobileTradeIn.Application.DTOs.Voucher;

public class UploadVoucherResponse
{
    public int TotalRecords { get; set; }

    public int SuccessRecords { get; set; }

    public int FailedRecords { get; set; }

    public string Message { get; set; } = string.Empty;
}