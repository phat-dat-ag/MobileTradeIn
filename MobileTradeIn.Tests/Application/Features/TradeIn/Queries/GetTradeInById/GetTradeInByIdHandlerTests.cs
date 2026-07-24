using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Features.TradeIn.Queries.GetTradeInById;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.Application.Features.TradeIn.Queries.GetTradeInById
{
    public class GetTradeInByIdHandlerTests
    {
        private readonly Mock<ITradeInRepository> _repositoryMock;
        private readonly Mock<ILogger<GetTradeInByIdHandler>> _loggerMock;

        private readonly GetTradeInByIdHandler _handler;
        public GetTradeInByIdHandlerTests()
        {
            _repositoryMock = new Mock<ITradeInRepository>();
            _loggerMock = new Mock<ILogger<GetTradeInByIdHandler>>();

            _handler = new GetTradeInByIdHandler(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_Should_GetTradeInById_When_RequestIsValid()
        {
            var command = new GetTradeInByIdQuery
            {
                TradeInOfferId = 1,
            };

            var response = new TradeInDto
            {
                TradeInOfferId = 1,
                OfferAmount = 100000,
                IMEI = "1111111111",
                OfferDate = DateTime.Now,
                VoucherCode = "CODE",
                OriginalAmount = 90000,
                VoucherAmount = 10000,
            };

            _repositoryMock.Setup(x => x.GetTradeInByIdAsync(It.IsAny<int>())).ReturnsAsync(response);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<TradeInDto>(result);
            Assert.Equal(command.TradeInOfferId, result.TradeInOfferId);

            _repositoryMock.Verify(x => x.GetTradeInByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowTradeInNotFoundException_When_TradeInNotFound()
        {
            var command = new GetTradeInByIdQuery
            {
                TradeInOfferId = 1,
            };

            await Assert.ThrowsAsync<TradeInNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _repositoryMock.Verify(x => x.GetTradeInByIdAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
