using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInCommandValidatorTests
{
    private readonly ConfirmTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new ConfirmTradeInCommand
        {
            TradeInOfferId = 1,
            ConfirmedBy = "admin",
            Notes = "Approved"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTradeInOfferIdIsLessThanOrEqualToZero()
    {
        var command = new ConfirmTradeInCommand
        {
            TradeInOfferId = 0,
            ConfirmedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.TradeInOfferId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenConfirmedByIsEmpty()
    {
        var command = new ConfirmTradeInCommand
        {
            TradeInOfferId = 1,
            ConfirmedBy = string.Empty
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ConfirmedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenConfirmedByExceedsMaximumLength()
    {
        var command = new ConfirmTradeInCommand
        {
            TradeInOfferId = 1,
            ConfirmedBy = new string('A', 101)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ConfirmedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNotesExceedMaximumLength()
    {
        var command = new ConfirmTradeInCommand
        {
            TradeInOfferId = 1,
            ConfirmedBy = "admin",
            Notes = new string('A', 501)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Notes));
    }
}