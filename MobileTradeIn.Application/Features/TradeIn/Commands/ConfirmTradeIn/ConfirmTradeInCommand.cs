using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInCommand : IRequest
{
    [Range(1, int.MaxValue)]
    public int TradeInOfferId { get; set; }

    [Required]
    public string ConfirmedBy { get; set; } = string.Empty;

    public string? Notes { get; set; }
}