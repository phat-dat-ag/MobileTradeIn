using Microsoft.Extensions.Logging;
using MobileTradeIn.Infrastructure.Services.FileReaders.Csv;
using Moq;
using System.Text;

namespace MobileTradeIn.Tests.Infrastructure.Services.FileReaders;

public class CsvFileReaderTests
{
    private readonly CsvFileReader _reader;

    public CsvFileReaderTests()
    {
        var logger = new Mock<ILogger<CsvFileReader>>();

        _reader = new CsvFileReader(logger.Object);
    }

    [Fact]
    public void CanRead_ShouldReturnTrue_WhenExtensionIsCsv()
    {
        Assert.True(_reader.CanRead(".csv"));
        Assert.True(_reader.CanRead(".CSV"));
    }

    [Fact]
    public void CanRead_ShouldReturnFalse_WhenExtensionIsNotCsv()
    {
        Assert.False(_reader.CanRead(".txt"));
        Assert.False(_reader.CanRead(".xlsx"));
    }

    [Fact]
    public void ReadAsync_ShouldReturnVoucherList_WhenCsvIsValid()
    {
        var csv = """
            VoucherCode,StartDate,EndDate
            ABC001,2026-01-01,2026-12-31
            ABC002,2026-02-01,2026-12-31
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        var result = _reader.ReadAsync(stream, 1, "admin");

        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].VoucherHeaderId);
        Assert.Equal("ABC001", result[0].VoucherCode);
        Assert.Equal("admin", result[0].CreatedBy);
        Assert.True(result[0].IsActive);

        Assert.Equal("ABC002", result[1].VoucherCode);
    }

    [Fact]
    public void ReadAsync_ShouldReturnEmptyList_WhenCsvContainsOnlyHeader()
    {
        var csv = """
            VoucherCode,StartDate,EndDate
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        var result = _reader.ReadAsync(stream, 1, "admin");

        Assert.Empty(result);
    }

    [Fact]
    public void ReadAsync_ShouldThrow_WhenHeaderIsInvalid()
    {
        var csv = """
            Code,Start,End
            ABC001,2026-01-01,2026-12-31
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        Assert.ThrowsAny<Exception>(() => _reader.ReadAsync(stream, 1, "admin"));
    }

    [Fact]
    public void ReadAsync_ShouldThrow_WhenDateFormatIsInvalid()
    {
        var csv = """
            VoucherCode,StartDate,EndDate
            ABC001,abc,xyz
            """;

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        Assert.ThrowsAny<Exception>(() => _reader.ReadAsync(stream, 1, "admin"));
    }
}