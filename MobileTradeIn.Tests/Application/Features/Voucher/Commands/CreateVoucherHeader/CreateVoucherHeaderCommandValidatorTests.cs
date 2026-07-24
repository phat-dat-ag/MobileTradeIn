using FluentValidation.Results;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderCommandValidatorTests
{
    private readonly CreateVoucherHeaderCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            Description = "Voucher batch",
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadFileIdIsLessThanOrEqualToZero()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 0,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadFileId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherBatchCodeIsEmpty()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = string.Empty,
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherBatchCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherBatchCodeExceedsMaximumLength()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = new string('A', 101),
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherBatchCode));
    }

    [Fact]
    public void Validate_ShouldFail_WhenProductIdIsLessThanOrEqualToZero()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 0,
            VoucherValue = 100000,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ProductId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherValueIsLessThanOrEqualToZero()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 0,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherValue));
    }

    [Fact]
    public void Validate_ShouldFail_WhenVoucherValueIsNotWholeNumber()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000.5m,
            Quantity = 100,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.VoucherValue));
    }

    [Fact]
    public void Validate_ShouldFail_WhenQuantityIsLessThanOrEqualToZero()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 0,
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Quantity));
    }

    [Fact]
    public void Validate_ShouldFail_WhenDescriptionExceedsMaximumLength()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            Description = new string('A', 501),
            CreatedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Description));
    }

    [Fact]
    public void Validate_ShouldFail_WhenCreatedByIsEmpty()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
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
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 1,
            VoucherValue = 100000,
            Quantity = 100,
            CreatedBy = new string('A', 101)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.CreatedBy));
    }
}