using FluentValidation;

namespace MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileCommandValidator
    : AbstractValidator<CreateUploadFileCommand>
{
    public CreateUploadFileCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required.")
            .MaximumLength(255)
            .WithMessage("FileName must not exceed 255 characters.");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("FilePath is required.")
            .MaximumLength(500)
            .WithMessage("FilePath must not exceed 500 characters.");

        RuleFor(x => x.FileType)
            .NotEmpty()
            .WithMessage("FileType is required.")
            .MaximumLength(20)
            .WithMessage("FileType must not exceed 20 characters.");

        RuleFor(x => x.UploadedBy)
            .NotEmpty()
            .WithMessage("UploadedBy is required.")
            .MaximumLength(100)
            .WithMessage("UploadedBy must not exceed 100 characters.");
    }
}