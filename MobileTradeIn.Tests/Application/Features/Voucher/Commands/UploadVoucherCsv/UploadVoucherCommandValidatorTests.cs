using FluentValidation.Results;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;
using MobileTradeIn.Tests.Common.Factories.Voucher;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.UploadVoucher;

public class UploadVoucherCommandValidatorTests
{
    private readonly UploadVoucherCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand(1, "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherHeaderIdIsLessThanOrEqualToZero()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand(0, "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherHeaderId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByIsEmpty()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand(1, string.Empty);

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByExceedsMaximumLength()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand(1, new string('A', 101));

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVouchersIsNull()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand(null!);

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Vouchers));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVouchersIsEmpty()
    {
        var command = UploadVoucherCommandFactory.CreateUploadVoucherCommand([]);

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Vouchers));
    }
}