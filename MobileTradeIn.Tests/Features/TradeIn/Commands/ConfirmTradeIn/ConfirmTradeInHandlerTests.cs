using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Application.Interfaces.Services;
using Moq;

namespace MobileTradeIn.Tests.Features.TradeIn.Commands.ConfirmTradeIn
{
    public class ConfirmTradeInHandlerTests
    {
        private readonly Mock<ITradeInRepository> _repositoryMock;
        private readonly Mock<ILogger<ConfirmTradeInHandler>> _loggerMock;
        private readonly Mock<IEmailTemplateRepository> _emailTemplateRepositoryMock;
        private readonly Mock<IEmailTemplateService> _emailTemplateServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        private readonly ConfirmTradeInHandler _handler;

        public ConfirmTradeInHandlerTests()
        {
            _repositoryMock = new Mock<ITradeInRepository>();
            _loggerMock = new Mock<ILogger<ConfirmTradeInHandler>>();
            _emailTemplateRepositoryMock = new Mock<IEmailTemplateRepository>();
            _emailTemplateServiceMock = new Mock<IEmailTemplateService>();
            _emailServiceMock = new Mock<IEmailService>();

            _handler = new ConfirmTradeInHandler(
                _repositoryMock.Object,
                _loggerMock.Object,
                _emailTemplateRepositoryMock.Object,
                _emailTemplateServiceMock.Object,
                _emailServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_ConfirmTradeIn_And_SendEmail()
        {
            var command = new ConfirmTradeInCommand
            {
                TradeInOfferId = 1,
                ConfirmedBy = "DAT"
            };

            var emailInfo = new TradeInEmailDto
            {
                CustomerName = "Nguyen Van A",
                CustomerEmail = "test@gmail.com",
                ProductName = "iPhone 15",
                OfferAmount = 10000000,
                TransactionNumber = "TRX0001"
            };

            var template = new EmailTemplateDto
            {
                Subject = "Approved",
                Content = "Hello {{CustomerName}}"
            };

            _repositoryMock
                .Setup(x => x.ConfirmTradeInAsync(It.IsAny<ConfirmTradeInRequest>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.GetTradeInEmailAsync(command.TradeInOfferId))
                .ReturnsAsync(emailInfo);

            _emailTemplateRepositoryMock
                .Setup(x => x.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED"))
                .ReturnsAsync(template);

            _emailTemplateServiceMock
                .Setup(x => x.RenderContentFromEmailTemplate(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns("Rendered");

            _emailServiceMock
                .Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            await _handler.Handle(command, CancellationToken.None);

            _repositoryMock.Verify(
                x => x.ConfirmTradeInAsync(It.IsAny<ConfirmTradeInRequest>()),
                Times.Once);

            _repositoryMock.Verify(
                x => x.GetTradeInEmailAsync(command.TradeInOfferId),
                Times.Once);

            _emailTemplateRepositoryMock.Verify(
                x => x.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED"),
                Times.Once);

            _emailTemplateServiceMock.Verify(
                x => x.RenderContentFromEmailTemplate(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()),
                Times.Exactly(2));

            _emailServiceMock.Verify(
                x => x.SendEmailAsync(
                    emailInfo.CustomerEmail,
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EmailInfo_NotFound()
        {
            var command = new ConfirmTradeInCommand
            {
                TradeInOfferId = 1,
                ConfirmedBy = "DAT"
            };

            _repositoryMock
                .Setup(x => x.ConfirmTradeInAsync(It.IsAny<ConfirmTradeInRequest>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.GetTradeInEmailAsync(command.TradeInOfferId))
                .ReturnsAsync((TradeInEmailDto?)null);

            await Assert.ThrowsAsync<EmailInforNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _emailServiceMock.Verify(
                x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_EmailTemplate_NotFound()
        {
            var command = new ConfirmTradeInCommand
            {
                TradeInOfferId = 1,
                ConfirmedBy = "DAT"
            };

            _repositoryMock
                .Setup(x => x.ConfirmTradeInAsync(It.IsAny<ConfirmTradeInRequest>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.GetTradeInEmailAsync(command.TradeInOfferId))
                .ReturnsAsync(new TradeInEmailDto());

            _emailTemplateRepositoryMock
                .Setup(x => x.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED"))
                .ReturnsAsync((EmailTemplateDto?)null);

            await Assert.ThrowsAsync<EmailTemplateNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _emailServiceMock.Verify(
                x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public async Task Handle_Should_NotThrow_When_SendEmail_Fails()
        {
            var command = new ConfirmTradeInCommand
            {
                TradeInOfferId = 1,
                ConfirmedBy = "DAT"
            };

            var template = new EmailTemplateDto
            {
                Subject = "Approved",
                Content = "Content"
            };

            _repositoryMock
                .Setup(x => x.ConfirmTradeInAsync(It.IsAny<ConfirmTradeInRequest>()))
                .Returns(Task.CompletedTask);

            _repositoryMock
                .Setup(x => x.GetTradeInEmailAsync(It.IsAny<int>()))
                .ReturnsAsync(new TradeInEmailDto
                {
                    CustomerEmail = "test@gmail.com"
                });

            _emailTemplateRepositoryMock
                .Setup(x => x.GetEmailTemplateByTemplateCodeAsync(It.IsAny<string>()))
                .ReturnsAsync(template);

            _emailTemplateServiceMock
                .Setup(x => x.RenderContentFromEmailTemplate(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>()))
                .Returns("Content");

            _emailServiceMock
                .Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            await _handler.Handle(command, CancellationToken.None);

            _emailServiceMock.Verify(
                x => x.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);
        }
    }
}
