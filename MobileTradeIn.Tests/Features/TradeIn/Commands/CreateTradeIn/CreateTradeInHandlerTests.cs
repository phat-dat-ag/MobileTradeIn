using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.TradeIn;

public class CreateTradeInHandlerTests
{
    private readonly Mock<ITradeInRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateTradeInHandler>> _loggerMock;

    private readonly CreateTradeInHandler _handler;

    public CreateTradeInHandlerTests()
    {
        _repositoryMock = new Mock<ITradeInRepository>();

        _loggerMock = new Mock<ILogger<CreateTradeInHandler>>();

        _handler = new CreateTradeInHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateTradeIn_When_RequestIsValid()
    {

        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            IMEI = "123456789",
            VoucherCode = null,
            CreatedBy = "DAT"
        };

        var response = new CreateTradeInResponse
        {
            TradeInRequestId = 1,
            TradeInOfferId = 1,
            OfferAmount = 10000000
        };

        _repositoryMock
            .Setup(x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()))
            .ReturnsAsync(response);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);

        Assert.Equal(1, result.TradeInRequestId);

        Assert.Equal(10000000, result.OfferAmount);

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidDeviceConditionException_When_DeviceCondition_Invalid()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "ABC",
            CreatedBy = "DAT"
        };

        await Assert.ThrowsAsync<InvalidDeviceConditionException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidVoucherCodeException_When_VoucherCode_IsWhiteSpace()
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = "GOOD",
            VoucherCode = "   ",
            CreatedBy = "DAT"
        };

        await Assert.ThrowsAsync<InvalidVoucherCodeException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Never);
    }

    [Theory]
    [InlineData("NEW")]
    [InlineData("GOOD")]
    [InlineData("FAIR")]
    [InlineData("POOR")]
    public async Task Handle_Should_Accept_All_Valid_DeviceConditions(string condition)
    {
        var command = new CreateTradeInCommand
        {
            CustomerId = 1,
            ProductId = 1,
            DeviceCondition = condition,
            CreatedBy = "DAT"
        };

        _repositoryMock
            .Setup(x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()))
            .ReturnsAsync(new CreateTradeInResponse());

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Once);
    }
}