using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Services;
using System.Globalization;

namespace MobileTradeIn.Infrastructure.Services;

public class CsvService : ICsvService
{
    private readonly ILogger<CsvService> _logger;

    public CsvService(ILogger<CsvService> logger)
    {
        _logger = logger;
    }
    public Task<List<VoucherImportDto>> ReadVoucherCsvAsync(Stream stream, int voucherHeaderId, string createdBy)
    {
        _logger.LogInformation(
            "Service Started. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}",
            "ReadVoucherCsv",
            voucherHeaderId);

        using var reader = new StreamReader(stream);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim
        };

        using var csv = new CsvReader(reader, config);

        var records = csv.GetRecords<VoucherCsvRecord>();

        var vouchers = new List<VoucherImportDto>();

        foreach (var record in records)
        {
            vouchers.Add(new VoucherImportDto
            {
                VoucherCode = record.VoucherCode,
                VoucherHeaderId = voucherHeaderId,
                StartDate = record.StartDate,
                EndDate = record.EndDate,
                IsActive = true,
                CreatedBy = createdBy
            });
        }

        _logger.LogInformation(
            "Service Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}. VoucherCount={VoucherCount}",
            "ReadVoucherCsv",
            voucherHeaderId,
            vouchers.Count);

        return Task.FromResult(vouchers);
    }
}