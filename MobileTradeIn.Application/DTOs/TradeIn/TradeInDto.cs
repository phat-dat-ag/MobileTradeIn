namespace MobileTradeIn.Application.DTOs.TradeIn;

public class TradeInDto
{
    public int TradeInOfferId { get; set; }

    public decimal OfferAmount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime OfferDate { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public string? VoucherCode { get; set; }

    public decimal OriginalAmount { get; set; }

    public decimal VoucherAmount { get; set; }

    public string DeviceCondition { get; set; } = string.Empty;

    public string? IMEI { get; set; }
}