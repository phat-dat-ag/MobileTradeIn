using MobileTradeIn.Application.Common.Exceptions.Business;

namespace MobileTradeIn.Tests.Infrastructure.Services;

public class FileValidatorTests
{
    private readonly FileValidator _validator = new();

    [Fact]
    public void ValidateFileName_ShouldThrowBusinessException_WhenFileNameIsEmpty()
    {
        var exception = Assert.Throws<BusinessException>(() =>
            _validator.ValidateFileName(string.Empty));

        Assert.Equal("File's name is empty.", exception.Message);
    }

    [Theory]
    [InlineData("Voucher_20260729.txt")]
    [InlineData("Voucher_23.csv")]
    [InlineData("Voucher20260729.csv")]
    [InlineData("Voucher_202607291.csv")]
    [InlineData("Voucher_.csv")]
    [InlineData("Voucher_abcdefgh.csv")]
    [InlineData("Voucher_20260729.csv.bak")]
    public void ValidateFileName_ShouldThrowBusinessException_WhenFileNameIsInvalid(
        string fileName)
    {
        var exception = Assert.Throws<BusinessException>(() =>
            _validator.ValidateFileName(fileName));

        Assert.Equal("Invalid file name.", exception.Message);
    }

    [Theory]
    [InlineData("Voucher_20260729.csv")]
    [InlineData("voucher_20250101.csv")]
    [InlineData("VOUCHER_12345678.csv")]
    [InlineData("VoUcHeR_00000001.csv")]
    public void ValidateFileName_ShouldNotThrowException_WhenFileNameIsValid(
        string fileName)
    {
        var exception = Record.Exception(() =>
            _validator.ValidateFileName(fileName));

        Assert.Null(exception);
    }
}