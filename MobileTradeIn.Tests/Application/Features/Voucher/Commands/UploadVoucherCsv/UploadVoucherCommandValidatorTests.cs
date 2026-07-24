using FluentValidation.Results;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.UploadVoucher;

public class UploadVoucherCommandValidatorTests
{
    private readonly UploadVoucherCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 1,
            UploadedBy = "admin",
            Vouchers =
            [
                new VoucherImportDto()
            ]
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadFieldIsLessThanOrEqualToZero()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 0,
            VoucherHeaderId = 1,
            UploadedBy = "admin",
            Vouchers =
            [
                new VoucherImportDto()
            ]
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadField));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherHeaderIdIsLessThanOrEqualToZero()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 0,
            UploadedBy = "admin",
            Vouchers =
            [
                new VoucherImportDto()
            ]
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherHeaderId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByIsEmpty()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 1,
            UploadedBy = string.Empty,
            Vouchers =
            [
                new VoucherImportDto()
            ]
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByExceedsMaximumLength()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 1,
            UploadedBy = new string('A', 101),
            Vouchers =
            [
                new VoucherImportDto()
            ]
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVouchersIsNull()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 1,
            UploadedBy = "admin",
            Vouchers = null!
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Vouchers));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVouchersIsEmpty()
    {
        var command = new UploadVoucherCommand
        {
            UploadField = 1,
            VoucherHeaderId = 1,
            UploadedBy = "admin",
            Vouchers = []
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Vouchers));
    }
}