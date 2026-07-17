using CsvHelper.Configuration.Attributes;
using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Tests.Application.DTOs.Voucher;

public class VoucherCsvRecordTests
{
    [Fact]
    public void VoucherCsvRecord_ShouldSetAndGetProperties()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var record = new VoucherCsvRecord
        {
            VoucherCode = "VC001",
            StartDate = today,
            EndDate = today.AddDays(30)
        };

        Assert.Equal("VC001", record.VoucherCode);
        Assert.Equal(today, record.StartDate);
        Assert.Equal(today.AddDays(30), record.EndDate);
    }

    [Fact]
    public void VoucherCode_ShouldHaveNameAttribute()
    {
        var property = typeof(VoucherCsvRecord)
            .GetProperty(nameof(VoucherCsvRecord.VoucherCode));

        var attribute = property!
            .GetCustomAttributes(typeof(NameAttribute), false)
            .Cast<NameAttribute>()
            .Single();

        Assert.Contains("VoucherCode", attribute.Names);
    }

    [Fact]
    public void StartDate_ShouldHaveNameAttribute()
    {
        var property = typeof(VoucherCsvRecord)
            .GetProperty(nameof(VoucherCsvRecord.StartDate));

        var attribute = property!
            .GetCustomAttributes(typeof(NameAttribute), false)
            .Cast<NameAttribute>()
            .Single();

        Assert.Contains("StartDate", attribute.Names);
    }

    [Fact]
    public void EndDate_ShouldHaveNameAttribute()
    {
        var property = typeof(VoucherCsvRecord)
            .GetProperty(nameof(VoucherCsvRecord.EndDate));

        var attribute = property!
            .GetCustomAttributes(typeof(NameAttribute), false)
            .Cast<NameAttribute>()
            .Single();

        Assert.Contains("EndDate", attribute.Names);
    }
}