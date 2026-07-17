using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.Conflict;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.Common.Exceptions.Validation;
using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucherCsv;
using MobileTradeIn.Application.Interfaces.Repositories;
using Moq;

namespace MobileTradeIn.Tests.Application.Features.Voucher.Commands.UploadVoucherCsv;

public class UploadVoucherCsvHandlerTests
{
    private readonly Mock<IVoucherRepository> _repositoryMock;
    private readonly Mock<ILogger<UploadVoucherCsvHandler>> _loggerMock;
    private readonly UploadVoucherCsvHandler _handler;

    public UploadVoucherCsvHandlerTests()
    {
        _repositoryMock = new Mock<IVoucherRepository>();
        _loggerMock = new Mock<ILogger<UploadVoucherCsvHandler>>();

        _handler = new UploadVoucherCsvHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    private static VoucherImportDto CreateVoucher(string code)
    {
        return new VoucherImportDto
        {
            VoucherCode = code,
            VoucherHeaderId = 1,
            StartDate = DateOnly.FromDateTime(DateTime.Today),
            EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
            IsActive = true,
            CreatedBy = "admin"
        };
    }

    [Fact]
    public async Task Handle_ShouldThrowVoucherHeaderNotFoundException_WhenHeaderDoesNotExist()
    {
        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers = new List<VoucherImportDto>
            {
                CreateVoucher("VC001")
            }
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync((VoucherHeaderDto?)null);

        await Assert.ThrowsAsync<VoucherHeaderNotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.GetVoucherHeaderAsync(command.VoucherHeaderId),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowNoVoucherException_WhenVoucherListIsEmpty()
    {
        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers = new List<VoucherImportDto>()
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = 0
            });

        await Assert.ThrowsAsync<NoVoucherException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowVoucherCountMismatch_WhenQuantityDoesNotMatch()
    {
        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers =
            [
                CreateVoucher("VC001")
            ]
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = 5
            });

        await Assert.ThrowsAsync<VoucherCountMismatch>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowDuplicateVoucherCodesException_WhenDuplicateCodesExist()
    {
        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers =
            [
                CreateVoucher("VC001"),
                CreateVoucher("vc001")
            ]
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = 2
            });

        await Assert.ThrowsAsync<DuplicateVoucherCodesException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowExistingVoucherCodeException_WhenVoucherAlreadyExists()
    {
        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers =
            [
                CreateVoucher("VC001"),
                CreateVoucher("VC002")
            ]
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = 2
            });

        _repositoryMock
            .Setup(x => x.GetExistingVoucherCodesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<string>
            {
                "VC001"
            });

        await Assert.ThrowsAsync<ExistingVoucherCodeException>(
            () => _handler.Handle(command, CancellationToken.None));

        _repositoryMock.Verify(
            x => x.GetExistingVoucherCodesAsync(It.IsAny<List<string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldImportSuccessfully_WhenVoucherCountLessThan500()
    {
        var vouchers = new List<VoucherImportDto>
        {
            CreateVoucher("VC001"),
            CreateVoucher("VC002")
        };

        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers = vouchers
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = vouchers.Count
            });

        _repositoryMock
            .Setup(x => x.GetExistingVoucherCodesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<string>());

        _repositoryMock
            .Setup(x => x.BulkInsertVoucherAsync(It.IsAny<List<VoucherImportDto>>()))
            .ReturnsAsync(vouchers.Count);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.TotalRecords);
        Assert.Equal(2, result.SuccessRecords);
        Assert.Equal(0, result.FailedRecords);
        Assert.Equal("Voucher import completed successfully.", result.Message);

        _repositoryMock.Verify(
            x => x.BulkInsertVoucherAsync(
                It.Is<List<VoucherImportDto>>(v => v.Count == 2)),
            Times.Once);

        _repositoryMock.Verify(
            x => x.UpdateUploadFileResultAsync(
                command.UploadField,
                2,
                2,
                0,
                "Voucher import completed successfully.",
                command.UploadedBy),
            Times.Once);

        _repositoryMock.Verify(
            x => x.MarkVoucherHeaderProcessedAsync(
                command.VoucherHeaderId,
                command.UploadedBy),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldImportInMultipleChunks_WhenVoucherCountGreaterThan500()
    {
        var vouchers = Enumerable.Range(1, 1001)
            .Select(i => CreateVoucher($"VC{i:0000}"))
            .ToList();

        var command = new UploadVoucherCsvCommand
        {
            VoucherHeaderId = 1,
            UploadField = 1,
            UploadedBy = "admin",
            Vouchers = vouchers
        };

        _repositoryMock
            .Setup(x => x.GetVoucherHeaderAsync(command.VoucherHeaderId))
            .ReturnsAsync(new VoucherHeaderDto
            {
                VoucherHeaderId = 1,
                Quantity = vouchers.Count
            });

        _repositoryMock
            .Setup(x => x.GetExistingVoucherCodesAsync(It.IsAny<List<string>>()))
            .ReturnsAsync(new List<string>());

        _repositoryMock
            .Setup(x => x.BulkInsertVoucherAsync(It.IsAny<List<VoucherImportDto>>()))
            .ReturnsAsync((List<VoucherImportDto> batch) => batch.Count);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(1001, result.TotalRecords);
        Assert.Equal(1001, result.SuccessRecords);
        Assert.Equal(0, result.FailedRecords);

        _repositoryMock.Verify(
            x => x.BulkInsertVoucherAsync(It.IsAny<List<VoucherImportDto>>()),
            Times.Exactly(3));

        _repositoryMock.Verify(
            x => x.UpdateUploadFileResultAsync(
                command.UploadField,
                1001,
                1001,
                0,
                "Voucher import completed successfully.",
                command.UploadedBy),
            Times.Once);

        _repositoryMock.Verify(
            x => x.MarkVoucherHeaderProcessedAsync(
                command.VoucherHeaderId,
                command.UploadedBy),
            Times.Once);
    }
}