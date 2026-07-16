using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;

namespace MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

public class GetTradeInByIdHandler : IRequestHandler<GetTradeInByIdQuery, TradeInDto?>
{
    private readonly ITradeInRepository _repository;
    private readonly ILogger<GetTradeInByIdHandler> _logger;

    public GetTradeInByIdHandler(ITradeInRepository repository, ILogger<GetTradeInByIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<TradeInDto?> Handle(
        GetTradeInByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Start getting TradeIn. TradeInOfferId={TradeInOfferId}",
            request.TradeInOfferId);

        var tradeIn = await _repository.GetTradeInByIdAsync(request.TradeInOfferId);

        if (tradeIn == null)
        {
            _logger.LogWarning(
                "Tradein not found. TradeInOfferId={TradeInOfferId}",
                request.TradeInOfferId);

            throw new TradeInNotFoundException(request.TradeInOfferId);
        }

        _logger.LogInformation(
            "TradeIn getting successfully. TradeInOfferId={TradeInOfferId}",
            request.TradeInOfferId);

        return tradeIn;
    }
}