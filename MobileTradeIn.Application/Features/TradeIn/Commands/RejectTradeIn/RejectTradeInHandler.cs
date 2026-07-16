using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInHandler
    : IRequestHandler<RejectTradeInCommand>
{
    private readonly ITradeInRepository _repository;
    private readonly ILogger<RejectTradeInHandler> _logger;

    public RejectTradeInHandler(ITradeInRepository repository, ILogger<RejectTradeInHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(RejectTradeInCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Rejecting TradeIn. OfferId={OfferId}",
            request.TradeInOfferId);

        await _repository.RejectTradeInAsync(
            new RejectTradeInRequest
            {
                TradeInOfferId = request.TradeInOfferId,
                RejectedBy = request.RejectedBy,
                Notes = request.Notes
            });

        _logger.LogInformation(
            "TradeIn rejected successfully. OfferId={OfferId}",
            request.TradeInOfferId);
    }
}