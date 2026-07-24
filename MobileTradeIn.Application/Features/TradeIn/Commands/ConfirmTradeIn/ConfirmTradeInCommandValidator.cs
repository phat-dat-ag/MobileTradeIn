using FluentValidation;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInCommandValidator
    : AbstractValidator<ConfirmTradeInCommand>
{
    public ConfirmTradeInCommandValidator()
    {
        RuleFor(x => x.TradeInOfferId)
            .GreaterThan(0)
            .WithMessage("TradeInOfferId must be greater than 0.");

        RuleFor(x => x.ConfirmedBy)
            .NotEmpty()
            .WithMessage("ConfirmedBy is required.")
            .MaximumLength(100)
            .WithMessage("ConfirmedBy must not exceed 100 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 500 characters.");
    }
}