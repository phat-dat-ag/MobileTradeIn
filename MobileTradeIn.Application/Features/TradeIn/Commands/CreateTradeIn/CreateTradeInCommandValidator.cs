using FluentValidation;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

public class CreateTradeInCommandValidator
    : AbstractValidator<CreateTradeInCommand>
{
    public CreateTradeInCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("CustomerId must be greater than 0.");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than 0.");

        RuleFor(x => x.DeviceCondition)
            .NotEmpty()
            .WithMessage("DeviceCondition is required.")
            .MaximumLength(100)
            .WithMessage("DeviceCondition must not exceed 100 characters.");

        RuleFor(x => x.IMEI)
            .NotEmpty()
            .WithMessage("IMEI is required.")
            .MaximumLength(50)
            .WithMessage("IMEI must not exceed 50 characters.");

        RuleFor(x => x.VoucherCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrWhiteSpace(x.VoucherCode))
            .WithMessage("VoucherCode must not exceed 50 characters.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy is required.")
            .MaximumLength(100)
            .WithMessage("CreatedBy must not exceed 100 characters.");
    }
}