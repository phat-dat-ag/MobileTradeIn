using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

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

    private void ValidateDeviceCondition(string deviceCondition, int customerId)
    {
        string[] validConditions =
        [
            "NEW",
            "GOOD",
            "FAIR",
            "POOR"
        ];

        if (!validConditions.Contains(deviceCondition.ToUpper()))
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. CustomerId={CustomerId}. DeviceCondition={DeviceCondition}",
                "ValidateDeviceCondition",
                customerId,
               deviceCondition);

            throw new InvalidDeviceConditionException(deviceCondition);
        }
    }

    private void ValidateVoucherCode(string? voucherCode, int customerId)
    {
        if (voucherCode != null && string.IsNullOrWhiteSpace(voucherCode))
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. CustomerId={CustomerId}. VoucherCode={VoucherCode}",
                "ValidateVoucherCode",
                customerId,
                voucherCode);

            throw new InvalidVoucherCodeException();
        }
    }

    public async Task<CreateTradeInResponse> Handle(
        CreateTradeInCommand request,
        CancellationToken cancellationToken)
    {
        ValidateDeviceCondition(request.DeviceCondition, request.CustomerId);

        ValidateVoucherCode(request.VoucherCode, request.CustomerId);

        _logger.LogInformation(
            "Business Started. Operation={Operation}. CustomerId={CustomerId}. ProductId={ProductId}",
            "CreateTradeIn",
            request.CustomerId,
            request.ProductId);

        var stopwatch = Stopwatch.StartNew();

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
            "Business Step Completed. Step={Step}. CustomerId={CustomerId}. ProductId={ProductId}",
            "CreateTradeInDatabase",
            request.CustomerId,
            request.ProductId);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. CustomerId={CustomerId}. ProductId={ProductId}. Elapsed={ElapsedMilliseconds}ms",
            "CreateTradeIn",
            request.CustomerId,
            request.ProductId,
            stopwatch.ElapsedMilliseconds);

        return result;
    }
}
