namespace MobileTradeIn.Application.DTOs.Voucher;

public class VoucherImportDto
{
    public string VoucherCode { get; set; } = string.Empty;

    public int VoucherHeaderId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}