using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Tests.Common.Factories.TradeIn;
using Moq;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Commands.CreateTradeIn;

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

        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", null!, "admin");

        var response = CreateTradeInResponseFactory.CreateCreateTradeInResponse();

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
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "ABC", "123456789012345", "VOUCHER01", "admin");

        await Assert.ThrowsAsync<InvalidDeviceConditionException>(() =>
            _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ThrowInvalidVoucherCodeException_When_VoucherCode_IsWhiteSpace()
    {
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, "GOOD", "123456789012345", "       ", "admin");

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
        var command = CreateTradeInCommandFactory.CreateCreateTradeInCommand(
            1, 1, condition, "123456789012345", "VOUCHER01", "admin");

        _repositoryMock
            .Setup(x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()))
            .ReturnsAsync(CreateTradeInResponseFactory.CreateCreateTradeInResponse());

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(
            x => x.CreateTradeInAsync(It.IsAny<CreateTradeInRequest>()),
            Times.Once);
    }
}