using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Queries.GetTradeInById;

public class GetTradeInByIdQueryValidatorTests
{
    private readonly GetTradeInByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenTradeInOfferIdIsValid()
    {
        var query = new GetTradeInByIdQuery
        {
            TradeInOfferId = 1
        };

        ValidationResult result = _validator.Validate(query);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTradeInOfferIdIsLessThanOrEqualToZero()
    {
        var query = new GetTradeInByIdQuery
        {
            TradeInOfferId = 0
        };

        ValidationResult result = _validator.Validate(query);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(query.TradeInOfferId));
    }
}