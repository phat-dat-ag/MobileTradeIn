using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;
using MobileTradeIn.Tests.Common.Factories.TradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInCommandValidatorTests
{
    private readonly CreateTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenCustomerIdIsLessThanOrEqualToZero()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            0, 1, "GOOD", "123456789012345", "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CustomerId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenProductIdIsLessThanOrEqualToZero()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 0, "GOOD", "123456789012345", "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ProductId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDeviceConditionIsEmpty()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, string.Empty, "123456789012345", "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.DeviceCondition));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDeviceConditionExceedsMaximumLength()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, new string('A', 101), "123456789012345", "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.DeviceCondition));
    }

    [Fact]
    public void Validate_ShouldFail_WhenIMEIIsEmpty()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", string.Empty, "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.IMEI));
    }

    [Fact]
    public void Validate_ShouldFail_WhenIMEIExceedsMaximumLength()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", new string('A', 51), "VOUCHER01", "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.IMEI));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherCodeExceedsMaximumLength()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", new string('A', 51), "admin");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByIsEmpty()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", "VOUCHER01", string.Empty);

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByExceedsMaximumLength()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", "VOUCHER01", new string('A', 101));

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }
}