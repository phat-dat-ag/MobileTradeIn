using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInCommand : IRequest
{
    [Range(1, int.MaxValue)]
    public int TradeInOfferId { get; set; }

    [Required]
    [MaxLength(100)]
    public string RejectedBy { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Notes { get; set; }
}