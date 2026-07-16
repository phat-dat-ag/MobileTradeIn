using MediatR;
using MobileTradeIn.Application.DTOs.TradeIn;
using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInCommand : IRequest<CreateTradeInResponse>
{

    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Required]
    public string DeviceCondition { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string? IMEI { get; set; }

    [MaxLength(50)]
    public string? VoucherCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string CreatedBy { get; set; } = string.Empty;
}
