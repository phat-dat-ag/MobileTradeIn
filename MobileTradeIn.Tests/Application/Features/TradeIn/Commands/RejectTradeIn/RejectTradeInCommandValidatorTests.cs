using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;
using MobileTradeIn.Tests.Common.Factories.TradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInCommandValidatorTests
{
    private readonly RejectTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = RejectTradeInCommandFactory.CreateRejectTradeInCommand(1, "DAT", "Customer reject");

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTradeInOfferIdIsLessThanOrEqualToZero()
    {
        var command = RejectTradeInCommandFactory.CreateRejectTradeInCommand(0, "DAT", "Customer reject");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.TradeInOfferId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenRejectedByIsEmpty()
    {
        var command = RejectTradeInCommandFactory.CreateRejectTradeInCommand(1, string.Empty, "Customer reject");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.RejectedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenRejectedByExceedsMaximumLength()
    {
        var command = RejectTradeInCommandFactory.CreateRejectTradeInCommand(1, new string('A', 101), "Customer reject");

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.RejectedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNotesExceedMaximumLength()
    {
        var command = RejectTradeInCommandFactory.CreateRejectTradeInCommand(1, "DAT", new string('A', 501));

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Notes));
    }
}