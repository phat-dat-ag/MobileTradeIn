using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Conflict;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;

namespace MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucherCsv;

public class UploadVoucherCsvHandler
    : IRequestHandler<UploadVoucherCsvCommand, UploadVoucherResponse>
{
    private readonly IVoucherRepository _repository;
    private readonly ILogger<UploadVoucherCsvHandler> _logger;

    public UploadVoucherCsvHandler(
        IVoucherRepository repository,
        ILogger<UploadVoucherCsvHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    private async Task<VoucherHeaderDto> GetAndValidateHeaderAsync(int voucherHeaderId)
    {
        var header = await _repository.GetVoucherHeaderAsync(voucherHeaderId);

        if (header == null)
        {
            _logger.LogWarning("VoucherHeader {HeaderId} not found.", voucherHeaderId);

            throw new VoucherHeaderNotFoundException();
        }

        return header;
    }

    private void ValidateVoucherCount(int requestVoucherCount, int headerVoucherQuantity)
    {
        if (requestVoucherCount == 0)
        {
            _logger.LogWarning("CSV contains no voucher.");

            throw new NoVoucherException();
        }

        if (requestVoucherCount != headerVoucherQuantity)
        {
            _logger.LogWarning(
                "Voucher quantity mismatch. Expected={Expected}, Actual={Actual}",
                headerVoucherQuantity,
                requestVoucherCount);

            throw new VoucherCountMismatch(headerVoucherQuantity, requestVoucherCount);
        }
    }

    private void ValidateDuplicateCodes(List<VoucherImportDto> vouchers)
    {
        var duplicateCodes = vouchers
            .GroupBy(req => req.VoucherCode.Trim().ToUpper())
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateCodes.Any())
        {
            _logger.LogWarning(
                "Duplicate voucher codes found in CSV: {VoucherCodes}",
                string.Join(", ", duplicateCodes));

            throw new DuplicateVoucherCodesException(string.Join(", ", duplicateCodes));
        }
    }

    private async Task ValidateExistingCodes(List<VoucherImportDto> vouchers)
    {
        var existingCodes = await _repository.GetExistingVoucherCodesAsync(
           vouchers
                .Select(req => req.VoucherCode)
                .ToList());

        if (existingCodes.Any())
        {
            _logger.LogWarning(
                "Voucher codes already exist: {VoucherCodes}",
                string.Join(", ", existingCodes));

            throw new ExistingVoucherCodeException(string.Join(", ", existingCodes));
        }
    }

    private async Task<int> ImportVouchersAsync(List<VoucherImportDto> vouchers)
    {
        int importedCount = 0;

        foreach (var batch in vouchers.Chunk(500))
        {
            importedCount += await _repository.BulkInsertVoucherAsync(
                batch.ToList());

            _logger.LogInformation(
                "Imported batch: {Count} vouchers.",
                batch.Count());
        }

        return importedCount;
    }

    private async Task UpdateImportResultAsync(UploadVoucherCsvCommand request, int importedCount)
    {
        await _repository.UpdateUploadFileResultAsync(
            request.UploadField,
            request.Vouchers.Count,
            importedCount,
            request.Vouchers.Count - importedCount,
            "Voucher import completed successfully.",
            request.UploadedBy);

        await _repository.MarkVoucherHeaderProcessedAsync(
            request.VoucherHeaderId,
            request.UploadedBy);
    }

    public async Task<UploadVoucherResponse> Handle(
        UploadVoucherCsvCommand request,
        CancellationToken cancellationToken)
    {

        var header = await GetAndValidateHeaderAsync(request.VoucherHeaderId);

        ValidateVoucherCount(request.Vouchers.Count, header.Quantity);

        ValidateDuplicateCodes(request.Vouchers);

        await ValidateExistingCodes(request.Vouchers);

        _logger.LogInformation(
            "Start importing {Count} vouchers. HeaderId={HeaderId}",
            request.Vouchers.Count,
            request.VoucherHeaderId);

        var importedCount = await ImportVouchersAsync(request.Vouchers);

        await UpdateImportResultAsync(request, importedCount);

        _logger.LogInformation(
            "Voucher import completed. Total={Total}, Success={Success}, Failed={Failed}",
            request.Vouchers.Count,
            importedCount,
            request.Vouchers.Count - importedCount);

        return new UploadVoucherResponse
        {
            TotalRecords = request.Vouchers.Count,
            SuccessRecords = importedCount,
            FailedRecords = request.Vouchers.Count - importedCount,
            Message = "Voucher import completed successfully."
        };
    }
}