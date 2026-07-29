using FluentValidation.Results;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;
using MobileTradeIn.Tests.Common.Factories.Voucher;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderCommandValidatorTests
{
    private readonly CreateVoucherHeaderCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
            "BATCH001", 1, 100000, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherBatchCodeIsEmpty()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           string.Empty, 1, 100000, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherBatchCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherBatchCodeExceedsMaximumLength()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           new string('A', 101), 1, 100000, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherBatchCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenProductIdIsLessThanOrEqualToZero()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           "BATCH001", 0, 100000, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ProductId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherValueIsLessThanOrEqualToZero()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           "BATCH001", 1, 0, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherValue));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherValueIsNotWholeNumber()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           "BATCH001", 1, 100000.5m, 100, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherValue));
    }

    [Fact]
    public void Validate_ShouldFail_WhenQuantityIsLessThanOrEqualToZero()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           "BATCH001", 1, 100000, 0, "Voucher batch", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Quantity));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDescriptionExceedsMaximumLength()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
           "BATCH001", 1, 100000, 100, new string('A', 501), "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Description));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByIsEmpty()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
            "BATCH001", 1, 100000, 100, "hehe", string.Empty);

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByExceedsMaximumLength()
    {
        var command = CreateVoucherHeaderCommandFactory.CreateCreateVoucherHeaderCommand(
            "BATCH001", 1, 100000, 100, "hehe", new string('A', 101));

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }
}