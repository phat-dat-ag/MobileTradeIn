using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.DTOs.UploadFile;
using MobileTradeIn.Application.Features.UploadFile.Commands.CreateUploadFile;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.Application.Features.UploadFile.Commands.CreateUploadFile;

public class CreateUploadFileHandlerTests
{
    private readonly Mock<IVoucherRepository> _repositoryMock;
    private readonly Mock<ILogger<CreateUploadFileHandler>> _loggerMock;
    private readonly CreateUploadFileHandler _handler;

    public CreateUploadFileHandlerTests()
    {
        _repositoryMock = new Mock<IVoucherRepository>();
        _loggerMock = new Mock<ILogger<CreateUploadFileHandler>>();

        _handler = new CreateUploadFileHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUploadFileSuccessfully()
    {
        var command = new CreateUploadFileCommand
        {
            FileName = "Voucher_20260717.csv",
            FilePath = @"C:\Uploads\Voucher_20260717.csv",
            FileType = "text/csv",
            UploadedBy = "admin"
        };

        var response = new CreateUploadFileResponse
        {
            UploadFileId = 1
        };

        _repositoryMock
            .Setup(x => x.CreateUploadFileAsync(
                It.IsAny<CreateUploadFileRequest>()))
            .ReturnsAsync(response);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(response.UploadFileId, result.UploadFileId);

        _repositoryMock.Verify(
            x => x.CreateUploadFileAsync(
                It.Is<CreateUploadFileRequest>(r =>
                    r.FileName == command.FileName &&
                    r.FilePath == command.FilePath &&
                    r.FileType == command.FileType &&
                    r.UploadedBy == command.UploadedBy)),
            Times.Once);

        _repositoryMock.VerifyNoOtherCalls();
    }
}