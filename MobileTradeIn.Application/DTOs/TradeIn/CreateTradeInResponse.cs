namespace MobileTradeIn.Application.DTOs.TradeIn;

public class CreateTradeInResponse
{
    public int TradeInRequestId { get; set; }

    public int TradeInOfferId { get; set; }

    public decimal OfferAmount { get; set; }

    public string Status { get; set; } = string.Empty;
}
