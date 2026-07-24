using CsvHelper.Configuration.Attributes;

namespace MobileTradeIn.Application.DTOs.Voucher;

public class VoucherCsvRecord
{
    [Name("VoucherCode")]
    public string VoucherCode { get; set; } = string.Empty;

    [Name("StartDate")]
    public DateOnly StartDate { get; set; }

    [Name("EndDate")]
    public DateOnly EndDate { get; set; }
}