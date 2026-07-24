using FluentValidation.Results;
using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.RejectTradeIn;

public class RejectTradeInCommandValidatorTests
{
    private readonly RejectTradeInCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldPass_WhenCommandIsValid()
    {
        var command = new RejectTradeInCommand
        {
            TradeInOfferId = 1,
            RejectedBy = "admin",
            Notes = "Rejected"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_ShouldFail_WhenTradeInOfferIdIsLessThanOrEqualToZero()
    {
        var command = new RejectTradeInCommand
        {
            TradeInOfferId = 0,
            RejectedBy = "admin"
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.TradeInOfferId));
    }

    [Fact]
    public void Validate_ShouldFail_WhenRejectedByIsEmpty()
    {
        var command = new RejectTradeInCommand
        {
            TradeInOfferId = 1,
            RejectedBy = string.Empty
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.RejectedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenRejectedByExceedsMaximumLength()
    {
        var command = new RejectTradeInCommand
        {
            TradeInOfferId = 1,
            RejectedBy = new string('A', 101)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.RejectedBy));
    }

    [Fact]
    public void Validate_ShouldFail_WhenNotesExceedMaximumLength()
    {
        var command = new RejectTradeInCommand
        {
            TradeInOfferId = 1,
            RejectedBy = "admin",
            Notes = new string('A', 501)
        };

        ValidationResult result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors,
            e => e.PropertyName == nameof(command.Notes));
    }
}