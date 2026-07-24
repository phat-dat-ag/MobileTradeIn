using MediatR;
using MobileTradeIn.Application.DTOs.TradeIn;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInCommand : IRequest<CreateTradeInResponse>
{

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public string DeviceCondition { get; set; } = string.Empty;

    public string? IMEI { get; set; }

    public string? VoucherCode { get; set; }

    public string CreatedBy { get; set; } = string.Empty;
}
