using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Conflict;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Repositories;
using System.Diagnostics;

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

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}",
            "GetVoucherHeader",
            voucherHeaderId);

        if (header == null)
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. VoucherHeaderId={VoucherHeaderId}",
                "GetVoucherHeader",
                voucherHeaderId);

            throw new VoucherHeaderNotFoundException();
        }

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}. ExpectedQuantity={ExpectedQuantity}",
            "ValidateVoucherHeader",
            voucherHeaderId,
            header.Quantity);

        return header;
    }

    private void ValidateVoucherCount(int requestVoucherCount, int headerVoucherQuantity)
    {
        if (requestVoucherCount == 0)
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}",
                "ValidateVoucherCount");

            throw new NoVoucherException();
        }

        if (requestVoucherCount != headerVoucherQuantity)
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. Expected={Expected}. Actual={Actual}",
                "ValidateVoucherCount",
                headerVoucherQuantity,
                requestVoucherCount);

            throw new VoucherCountMismatch(headerVoucherQuantity, requestVoucherCount);
        }

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. VoucherCount={VoucherCount}",
            "ValidateVoucherCount",
            requestVoucherCount);
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
                "Business Failed. Step={Step}. VoucherCodes={VoucherCodes}",
                "ValidateDuplicateCodes",
                string.Join(", ", duplicateCodes));

            throw new DuplicateVoucherCodesException(string.Join(", ", duplicateCodes));
        }

        _logger.LogInformation(
            "Business Step Completed. Step={Step}",
            "ValidateDuplicateCodes");
    }

    private async Task ValidateExistingCodes(List<VoucherImportDto> vouchers)
    {
        var existingCodes = await _repository.GetExistingVoucherCodesAsync(
           vouchers
                .Select(req => req.VoucherCode)
                .ToList());

        _logger.LogInformation(
                "Business Step Completed. Step={Step}",
                "GetExistingVoucherCodes");

        if (existingCodes.Any())
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. VoucherCodes={VoucherCodes}",
                "ValidateExistingVoucherCodes",
                string.Join(", ", existingCodes));

            throw new ExistingVoucherCodeException(string.Join(", ", existingCodes));
        }

        _logger.LogInformation(
            "Business Step Completed. Step={Step}",
            "ValidateExistingVoucherCodes");
    }

    private async Task<int> ImportVouchersAsync(List<VoucherImportDto> vouchers)
    {
        int importedCount = 0;

        foreach (var batch in vouchers.Chunk(500))
        {
            importedCount += await _repository.BulkInsertVoucherAsync(
                batch.ToList());

            _logger.LogInformation(
                "Business Step Completed. Step={Step}. BatchSize={BatchSize}",
                "ImportVoucherBatch",
                batch.Count());
        }

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. ImportedCount={ImportedCount}",
            "ImportVouchers",
            importedCount);

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

        _logger.LogInformation(
            "Business Step Completed. Step={Step}",
            "UpdateUploadFileResult");

        await _repository.MarkVoucherHeaderProcessedAsync(
            request.VoucherHeaderId,
            request.UploadedBy);

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}",
            "MarkVoucherHeaderProcessed",
            request.VoucherHeaderId);
    }

    public async Task<UploadVoucherResponse> Handle(
        UploadVoucherCsvCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Business Started. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}. VoucherCount={VoucherCount}",
            "UploadVoucherCsv",
            request.VoucherHeaderId,
            request.Vouchers.Count);

        var stopwatch = Stopwatch.StartNew();

        var header = await GetAndValidateHeaderAsync(request.VoucherHeaderId);

        ValidateVoucherCount(request.Vouchers.Count, header.Quantity);

        ValidateDuplicateCodes(request.Vouchers);

        await ValidateExistingCodes(request.Vouchers);

        var importedCount = await ImportVouchersAsync(request.Vouchers);

        await UpdateImportResultAsync(request, importedCount);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}. Total={Total}. Success={Success}. Failed={Failed}. Elapsed={ElapsedMilliseconds}ms",
            "UploadVoucherCsv",
            request.VoucherHeaderId,
            request.Vouchers.Count,
            importedCount,
            request.Vouchers.Count - importedCount,
            stopwatch.ElapsedMilliseconds);

        return new UploadVoucherResponse
        {
            TotalRecords = request.Vouchers.Count,
            SuccessRecords = importedCount,
            FailedRecords = request.Vouchers.Count - importedCount,
            Message = "Voucher import completed successfully."
        };
    }
}