namespace MobileTradeIn.Application.DTOs.TradeIn;

public class RejectTradeInRequest
{
    public int TradeInOfferId { get; set; }

    public string RejectedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}