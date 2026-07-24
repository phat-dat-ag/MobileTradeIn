using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInCommandValidatorTests
{
    private readonly CreateTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            VoucherCode = "VOUCHER01",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenCustomerIdIsLessThanOrEqualToZero()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 0,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CustomerId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenProductIdIsLessThanOrEqualToZero()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 0,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ProductId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDeviceConditionIsEmpty()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = string.Empty,
            IMEI = "123456789012345",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.DeviceCondition));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDeviceConditionExceedsMaximumLength()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = new string('A', 101),
            IMEI = "123456789012345",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.DeviceCondition));
    }

    [Fact]
    public void Validate_ShouldFail_WhenIMEIIsEmpty()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = string.Empty,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.IMEI));
    }

    [Fact]
    public void Validate_ShouldFail_WhenIMEIExceedsMaximumLength()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = new string('A', 51),
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.IMEI));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherCodeExceedsMaximumLength()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            VoucherCode = new string('A', 51),
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByIsEmpty()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            CreatedBy = string.Empty
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByExceedsMaximumLength()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789012345",
            CreatedBy = new string('A', 101)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }
}