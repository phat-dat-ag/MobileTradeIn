using FluentValidation;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInCommandValidator
    : AbstractValidator<RejectTradeInCommand>
{
    public RejectTradeInCommandValidator()
    {
        RuleFor(x => x.TradeInOfferId)
            .GreaterThan(0)
            .WithMessage("TradeInOfferId must be greater than 0.");

        RuleFor(x => x.RejectedBy)
            .NotEmpty()
            .WithMessage("RejectedBy is required.")
            .MaximumLength(100)
            .WithMessage("RejectedBy must not exceed 100 characters.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Notes))
            .WithMessage("Notes must not exceed 500 characters.");
    }
}