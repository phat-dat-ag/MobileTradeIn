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
            "Business Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "GetTradeInById",
            request.TradeInOfferId);

        var stopwatch = Stopwatch.StartNew();

        var tradeIn = await _repository.GetTradeInByIdAsync(request.TradeInOfferId);

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}",
            "GetTradeInByIdDatabase",
            request.TradeInOfferId);

        if (tradeIn == null)
        {
            stopwatch.Stop();

            _logger.LogWarning(
                "Business Failed. Step={Step}. TradeInOfferId={TradeInOfferId}. Elapsed={ElapsedMilliseconds}ms",
                "GetTradeInByIdDatabase",
                request.TradeInOfferId,
                stopwatch.ElapsedMilliseconds);

            throw new TradeInNotFoundException(request.TradeInOfferId);
        }

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}. Elapsed={ElapsedMilliseconds}ms",
            "GetTradeInById",
            request.TradeInOfferId,
            stopwatch.ElapsedMilliseconds);

        return tradeIn;
    }
}