using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

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
            "Business Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "RejectTradeIn",
            request.TradeInOfferId);

        var stopwatch = Stopwatch.StartNew();

        await _repository.RejectTradeInAsync(
            new RejectTradeInRequest
            {
                TradeInOfferId = request.TradeInOfferId,
                RejectedBy = request.RejectedBy,
                Notes = request.Notes
            });

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}",
            "RejectTradeInDatabase",
            request.TradeInOfferId);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}. Elapsed={ElapsedMilliseconds}ms",
            "RejectTradeIn",
            request.TradeInOfferId,
            stopwatch.ElapsedMilliseconds);
    }
}