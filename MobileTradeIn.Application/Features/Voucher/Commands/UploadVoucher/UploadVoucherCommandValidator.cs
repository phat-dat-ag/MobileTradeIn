using FluentValidation;

namespace MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;

public class UploadVoucherCommandValidator
    : AbstractValidator<UploadVoucherCommand>
{
    public UploadVoucherCommandValidator()
    {
        RuleFor(x => x.UploadField)
            .GreaterThan(0)
            .WithMessage("UploadField must be greater than 0.");

        RuleFor(x => x.VoucherHeaderId)
            .GreaterThan(0)
            .WithMessage("VoucherHeaderId must be greater than 0.");

        RuleFor(x => x.UploadedBy)
            .NotEmpty()
            .WithMessage("UploadedBy is required.")
            .MaximumLength(100)
            .WithMessage("UploadedBy must not exceed 100 characters.");

        RuleFor(x => x.Vouchers)
            .NotNull()
            .WithMessage("Vouchers are required.")
            .NotEmpty()
            .WithMessage("At least one voucher is required.");
    }
}