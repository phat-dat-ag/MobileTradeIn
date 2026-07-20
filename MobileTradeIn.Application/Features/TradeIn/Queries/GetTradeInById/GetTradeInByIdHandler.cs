using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

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
           "Starting trade-in retrieval. TradeInOfferId={TradeInOfferId}",
           request.TradeInOfferId);

        var stopwatch = Stopwatch.StartNew();

        var tradeIn = await _repository.GetTradeInByIdAsync(request.TradeInOfferId);

        if (tradeIn == null)
        {
            _logger.LogWarning(
               "Trade-in not found. TradeInOfferId={TradeInOfferId}. ElapsedMilliseconds={ElapsedMilliseconds}",
               request.TradeInOfferId,
               stopwatch.ElapsedMilliseconds);

            throw new TradeInNotFoundException(request.TradeInOfferId);
        }

        stopwatch.Stop();

        _logger.LogInformation(
            "Trade-in retrieval completed in {ElapsedMilliseconds} ms. TradeInOfferId={TradeInOfferId}",
            stopwatch.ElapsedMilliseconds,
            request.TradeInOfferId);

        return tradeIn;
    }
}