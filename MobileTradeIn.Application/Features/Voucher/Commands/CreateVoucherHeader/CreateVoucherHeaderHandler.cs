using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;

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
            "Creating voucher batch {BatchCode} for Product {ProductId}",
            request.VoucherBatchCode,
            request.ProductId);

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
            "Voucher batch created successfully. HeaderId={HeaderId}",
            result.VoucherHeaderId);

        return result;
    }
}