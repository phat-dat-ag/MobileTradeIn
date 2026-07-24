using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Interfaces.Services;
using System.Globalization;

namespace MobileTradeIn.Infrastructure.Services.FileReaders.Csv;

public class CsvFileReader : IFileReader
{
    private readonly ILogger<CsvFileReader> _logger;

    public CsvFileReader(ILogger<CsvFileReader> logger)
    {
        _logger = logger;
    }

    public bool CanRead(string extension)
    {
        return extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);
    }

    public List<VoucherImportDto> ReadAsync(Stream stream, int voucherHeaderId, string createdBy)
    {
        _logger.LogInformation(
            "Service Started. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}",
            "IFileReader",
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
                VoucherHeaderId = voucherHeaderId,
                VoucherCode = record.VoucherCode,
                StartDate = record.StartDate,
                EndDate = record.EndDate,
                IsActive = true,
                CreatedBy = createdBy
            });
        }

        _logger.LogInformation(
            "Service Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}. VoucherCount={VoucherCount}",
            "IFileReader",
            voucherHeaderId,
            vouchers.Count);

        return vouchers;
    }
}