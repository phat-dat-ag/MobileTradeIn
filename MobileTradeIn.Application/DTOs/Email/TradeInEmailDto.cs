namespace MobileTradeIn.Application.DTOs.Email;

public class TradeInEmailDto
{
    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string ProductName { get; set; } = string.Empty;

    public decimal OfferAmount { get; set; }

    public string TransactionNumber { get; set; } = string.Empty;
}