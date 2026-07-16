namespace MobileTradeIn.Application.DTOs.TradeIn;

public class ConfirmTradeInRequest
{
    public int TradeInOfferId { get; set; }

    public string ConfirmedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}
