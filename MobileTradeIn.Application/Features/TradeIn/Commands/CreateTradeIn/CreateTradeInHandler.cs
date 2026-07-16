using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInHandler
    : IRequestHandler<CreateTradeInCommand, CreateTradeInResponse>
{
    private readonly ITradeInRepository _repository;
    private readonly ILogger<CreateTradeInHandler> _logger;


    public CreateTradeInHandler(ITradeInRepository repository, ILogger<CreateTradeInHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateTradeInResponse> Handle(
        CreateTradeInCommand request,
        CancellationToken cancellationToken)
    {
        string[] validConditions =
        [
            "NEW",
            "GOOD",
            "FAIR",
            "POOR"
        ];

        if (!validConditions.Contains(request.DeviceCondition.ToUpper()))
        {
            _logger.LogWarning(
                "Invalid device condition. CustomerId={CustomerId}, DeviceCondition={DeviceCondition}",
                request.CustomerId,
                request.DeviceCondition);

            throw new InvalidDeviceConditionException(request.DeviceCondition);
        }

        if (request.VoucherCode != null && string.IsNullOrWhiteSpace(request.VoucherCode))
        {
            _logger.LogWarning(
                "Invalid voucher code. CustomerId={CustomerId}, VoucherCode={VoucherCode}",
                request.CustomerId,
                request.VoucherCode);

            throw new InvalidVoucherCodeException();
        }

        _logger.LogInformation(
            "Start creating TradeIn. CustomerId={CustomerId}, ProductId={ProductId}",
            request.CustomerId,
            request.ProductId);

        var result = await _repository.CreateTradeInAsync(
            new CreateTradeInRequest
            {
                CustomerId = request.CustomerId,
                ProductId = request.ProductId,
                DeviceCondition = request.DeviceCondition,
                IMEI = request.IMEI,
                VoucherCode = request.VoucherCode,
                CreatedBy = request.CreatedBy
            });

        _logger.LogInformation(
            "TradeIn created successfully.");

        return result;
    }
}
