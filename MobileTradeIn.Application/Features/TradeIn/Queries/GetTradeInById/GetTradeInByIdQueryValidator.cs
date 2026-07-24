using FluentValidation;

namespace MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

public class GetTradeInByIdQueryValidator
    : AbstractValidator<GetTradeInByIdQuery>
{
    public GetTradeInByIdQueryValidator()
    {
        RuleFor(x => x.TradeInOfferId)
            .GreaterThan(0)
            .WithMessage("TradeInOfferId must be greater than 0.");
    }
}