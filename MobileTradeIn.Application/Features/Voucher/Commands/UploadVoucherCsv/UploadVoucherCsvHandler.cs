using MediatR;
using Microsoft.Extensions.Logging;
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

    public async Task<UploadVoucherResponse> Handle(
        UploadVoucherCsvCommand request,
        CancellationToken cancellationToken)
    {

        var header = await _repository.GetVoucherHeaderAsync(request.VoucherHeaderId);

        if (header == null)
        {
            _logger.LogWarning("VoucherHeader {HeaderId} not found.", request.VoucherHeaderId);

            throw new KeyNotFoundException("Voucher header not found.");
        }

        if (request.Vouchers.Count == 0)
        {
            _logger.LogWarning("CSV contains no voucher.");

            throw new ArgumentException("CSV contains no voucher.");
        }

        if (request.Vouchers.Count != header.Quantity)
        {
            _logger.LogWarning(
                "Voucher quantity mismatch. Expected={Expected}, Actual={Actual}",
                header.Quantity,
                request.Vouchers.Count);

            throw new ArgumentException(
                $"Expected {header.Quantity} vouchers but found {request.Vouchers.Count}.");
        }

        var duplicateCodes = request.Vouchers
            .GroupBy(req => req.VoucherCode.Trim().ToUpper())
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateCodes.Any())
        {
            _logger.LogWarning(
                "Duplicate voucher codes found in CSV: {VoucherCodes}",
                string.Join(", ", duplicateCodes));

            throw new ArgumentException(
                $"Duplicate voucher codes found in CSV: {string.Join(", ", duplicateCodes)}");
        }

        var existingCodes = await _repository.GetExistingVoucherCodesAsync(
            request.Vouchers
                .Select(req => req.VoucherCode)
                .ToList());

        if (existingCodes.Any())
        {
            _logger.LogWarning(
                "Voucher codes already exist: {VoucherCodes}",
                string.Join(", ", existingCodes));

            throw new ArgumentException(
                $"Voucher codes already exist: {string.Join(", ", existingCodes)}");
        }

        _logger.LogInformation(
            "Start importing {Count} vouchers. HeaderId={HeaderId}",
            request.Vouchers.Count,
            request.VoucherHeaderId);

        int success = 0;

        foreach (var batch in request.Vouchers.Chunk(500))
        {
            success += await _repository.BulkInsertVoucherAsync(
                batch.ToList());

            _logger.LogInformation(
                "Imported batch: {Count} vouchers.",
                batch.Count());
        }

        await _repository.UpdateUploadFileResultAsync(
            request.UploadField,
            request.Vouchers.Count,
            success,
            request.Vouchers.Count - success,
            "Voucher import completed successfully.",
            request.UploadedBy);

        await _repository.MarkVoucherHeaderProcessedAsync(
            request.VoucherHeaderId,
            request.UploadedBy);

        _logger.LogInformation(
            "Voucher import completed. Total={Total}, Success={Success}, Failed={Failed}",
            request.Vouchers.Count,
            success,
            request.Vouchers.Count - success);

        return new UploadVoucherResponse
        {
            TotalRecords = request.Vouchers.Count,
            SuccessRecords = success,
            FailedRecords = request.Vouchers.Count - success,
            Message = "Voucher import completed successfully."
        };
    }
}