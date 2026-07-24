using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

namespace MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderHandler
    : IRequestHandler<CreateVoucherHeaderCommand, CreateVoucherHeaderResponse>
{
    private readonly IVoucherRepository _repository;
    private readonly ILogger<CreateVoucherHeaderHandler> _logger;

    public CreateVoucherHeaderHandler(IVoucherRepository repository, ILogger<CreateVoucherHeaderHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CreateVoucherHeaderResponse> Handle(
        CreateVoucherHeaderCommand request,
        CancellationToken cancellationToken)
    {

        _logger.LogInformation(
            "Business Started. Operation={Operation}. VoucherBatchCode={VoucherBatchCode}. ProductId={ProductId}. Quantity={Quantity}",
            "CreateVoucherHeader",
            request.VoucherBatchCode,
            request.ProductId,
            request.Quantity);

        var stopwatch = Stopwatch.StartNew();

        var result = await _repository.CreateVoucherHeaderAsync(
            new CreateVoucherHeaderRequest
            {
                UploadFileId = request.UploadFileId,
                VoucherBatchCode = request.VoucherBatchCode,
                ProductId = request.ProductId,
                VoucherValue = request.VoucherValue,
                Quantity = request.Quantity,
                Description = request.Description,
                CreatedBy = request.CreatedBy
            });

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}",
            "CreateVoucherHeaderDatabase",
            result.VoucherHeaderId);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}. Elapsed={ElapsedMilliseconds}ms",
            "CreateVoucherHeader",
            result.VoucherHeaderId,
            stopwatch.ElapsedMilliseconds);

        return result;
    }
}