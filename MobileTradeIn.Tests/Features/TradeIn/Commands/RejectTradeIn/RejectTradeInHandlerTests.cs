using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.RejectTradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.Features.TradeIn.Commands.RejectTradeIn
{
    public class RejectTradeInHandlerTests
    {
        private readonly Mock<ITradeInRepository> _repositoryMock;
        private readonly Mock<ILogger<RejectTradeInHandler>> _loggerMock;

        private readonly RejectTradeInHandler _handler;

        public RejectTradeInHandlerTests()
        {
            _repositoryMock = new Mock<ITradeInRepository>();
            _loggerMock = new Mock<ILogger<RejectTradeInHandler>>();

            _handler = new RejectTradeInHandler(
                _repositoryMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_RejectTradeIn_When_RequestIsValid()
        {

            var command = new RejectTradeInCommand
            {
                TradeInOfferId = 1,
                RejectedBy = "DAT",
                Notes = "Customer rejected."
            };

            _repositoryMock
                .Setup(x => x.RejectTradeInAsync(It.IsAny<RejectTradeInRequest>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(
                x => x.RejectTradeInAsync(
                    It.Is<RejectTradeInRequest>(r =>
                        r.TradeInOfferId == command.TradeInOfferId &&
                        r.RejectedBy == command.RejectedBy &&
                        r.Notes == command.Notes)),
                Times.Once);
        }
    }
}
