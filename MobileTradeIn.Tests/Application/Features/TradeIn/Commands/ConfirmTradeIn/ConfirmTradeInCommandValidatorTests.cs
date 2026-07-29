using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;
using MobileTradeIn.Tests.Common.Factories.TradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInCommandValidatorTests
{
    private readonly ConfirmTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = ConfirmTradeInCommandFactory.CreateConfirmTradeInCommand(1, "admin", "Approved");

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTradeInOfferIdIsLessThanOrEqualToZero()
    {
        var command = ConfirmTradeInCommandFactory.CreateConfirmTradeInCommand(0, "admin", "Approved");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.TradeInOfferId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenConfirmedByIsEmpty()
    {
        var command = ConfirmTradeInCommandFactory.CreateConfirmTradeInCommand(1, string.Empty, "Approved");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ConfirmedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenConfirmedByExceedsMaximumLength()
    {
        var command = ConfirmTradeInCommandFactory.CreateConfirmTradeInCommand(1, new string('A', 101), "Approved");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.ConfirmedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNotesExceedMaximumLength()
    {
        var command = ConfirmTradeInCommandFactory.CreateConfirmTradeInCommand(1, "admin", new string('A', 501));

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Notes));
    }
}