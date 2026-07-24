using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderHandlerTests
{
    private readonly Mock<IVoucherRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateVoucherHeaderHandler>> _loggerMock;
    private readonly CreateVoucherHeaderHandler _handler;

    public CreateVoucherHeaderHandlerTests()
    {
        _repositoryMock = new Mock<IVoucherRepository>();
        _loggerMock = new Mock<ILogger<CreateVoucherHeaderHandler>>();

        _handler = new CreateVoucherHeaderHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateVoucherHeaderSuccessfully()
    {
        var command = new CreateVoucherHeaderCommand
        {
            UploadFileId = 1,
            VoucherBatchCode = "BATCH001",
            ProductId = 10,
            VoucherValue = 500000,
            Quantity = 100,
            Description = "Voucher batch for iPhone",
            CreatedBy = "admin"
        };

        var response = new CreateVoucherHeaderResponse
        {
            VoucherHeaderId = 100,
            VoucherBatchCode = "BATCH001",
            Message = "Voucher batch created successfully."
        };

        _repositoryMock
            .Setup(x => x.CreateVoucherHeaderAsync(
                It.IsAny<CreateVoucherHeaderRequest>()))
            .ReturnsAsync(response);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(response.VoucherHeaderId, result.VoucherHeaderId);
        Assert.Equal(response.VoucherBatchCode, result.VoucherBatchCode);
        Assert.Equal(response.Message, result.Message);

        _repositoryMock.Verify(
            x => x.CreateVoucherHeaderAsync(
                It.Is<CreateVoucherHeaderRequest>(r =>
                    r.UploadFileId == command.UploadFileId &&
                    r.VoucherBatchCode == command.VoucherBatchCode &&
                    r.ProductId == command.ProductId &&
                    r.VoucherValue == command.VoucherValue &&
                    r.Quantity == command.Quantity &&
                    r.Description == command.Description &&
                    r.CreatedBy == command.CreatedBy)),
            Times.Once);
    }
}