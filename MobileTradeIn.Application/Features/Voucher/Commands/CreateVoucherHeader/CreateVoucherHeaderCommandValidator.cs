using FluentValidation;

namespace MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderCommandValidator
    : AbstractValidator<CreateVoucherHeaderCommand>
{
    public CreateVoucherHeaderCommandValidator()
    {
        RuleFor(x => x.UploadFileId)
            .GreaterThan(0)
            .WithMessage("UploadFileId must be greater than 0.");

        RuleFor(x => x.VoucherBatchCode)
            .NotEmpty()
            .WithMessage("VoucherBatchCode is required.")
            .MaximumLength(100)
            .WithMessage("VoucherBatchCode must not exceed 100 characters.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.VoucherValue)
            .GreaterThan(0)
            .WithMessage("VoucherValue must be greater than 0.")
            .Must(x => decimal.Truncate(x) == x)
            .WithMessage("VoucherValue must be a whole number.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy is required.")
            .MaximumLength(100)
            .WithMessage("CreatedBy must not exceed 100 characters.");
    }
}