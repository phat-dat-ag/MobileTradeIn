using FluentValidation.Results;
using MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

namespace MobileTradeIn.Tests.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileCommandValidatorTests
{
    private readonly CreateUploadFileCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = "csv",
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenFileNameIsEmpty()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = string.Empty,
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = "csv",
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FileName));
    }

    [Fact]
    public void Validate_ShouldFail_WhenFileNameExceedsMaximumLength()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = new string('A', 256),
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = "csv",
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FileName));
    }

    [Fact]
    public void Validate_ShouldFail_WhenFilePathIsEmpty()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = string.Empty,
            FileType = "csv",
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FilePath));
    }

    [Fact]
    public void Validate_ShouldFail_WhenFilePathExceedsMaximumLength()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = new string('A', 501),
            FileType = "csv",
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FilePath));
    }

    [Fact]
    public void Validate_ShouldFail_WhenFileTypeIsEmpty()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = string.Empty,
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FileType));
    }

    [Fact]
    public void Validate_ShouldFail_WhenFileTypeExceedsMaximumLength()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = new string('A', 21),
            UploadedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.FileType));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByIsEmpty()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = "csv",
            UploadedBy = string.Empty
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenUploadedByExceedsMaximumLength()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "voucher.csv",
            FilePath = @"C:\Uploads\voucher.csv",
            FileType = "csv",
            UploadedBy = new string('A', 101)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.UploadedBy));
    }
}