namespace MobileTradeIn.Application.DTOs.TradeIn;

public class CreateTradeInRequest
{
    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string DeviceCondition { get; set; } = string.Empty;

    public string? IMEI { get; set; }

    public string? VoucherCode { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}