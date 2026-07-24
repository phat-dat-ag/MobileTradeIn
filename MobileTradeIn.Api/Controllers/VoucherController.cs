using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Api.Models;
using MobileTradeIn.Application.Common.Exceptions.Business;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;
using MobileTradeIn.Application.Interfaces.Services;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/vouchers")]
public class VoucherController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<VoucherController> _logger;
    private readonly IEnumerable<IFileReader> _fileReaders;

    public VoucherController(IMediator mediator, IEnumerable<IFileReader> fileReaders, ILogger<VoucherController> logger)
    {
        _mediator = mediator;
        _fileReaders = fileReaders;
        _logger = logger;
    }

    [HttpPost("header")]
    public async Task<IActionResult> CreateHeader(
       [FromBody] CreateVoucherHeaderCommand command)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}",
            "CreateVoucherHeader");

        var result = await _mediator.Send(command);

        _logger.LogInformation(
            "API Completed. Operation={Operation}",
            "CreateVoucherHeader");

        return Success(result, "VoucherHeader created successfully.");
    }

    [HttpPost("header/{headerId}/upload")]
    public async Task<IActionResult> UploadVoucher(
        int headerId,
        [FromForm] UploadVoucherRequest request)
    {
        _logger.LogInformation(
            "API Started. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}",
            "UploadVoucher",
            headerId);

        var extension = Path.GetExtension(request.File.FileName);

        var reader = _fileReaders
            .FirstOrDefault(x => x.CanRead(extension));

        if (reader == null)
        {
            throw new BusinessException(
                $"File extension '{extension}' is not supported.");
        }

        using var stream = request.File.OpenReadStream();

        var vouchers = reader.ReadAsync(
            stream,
            headerId,
            request.UploadedBy);

        _logger.LogInformation(
            "API Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}. VoucherCount={VoucherCount}",
            "ReadVoucher",
            headerId,
            vouchers.Count);

        var result = await _mediator.Send(
            new UploadVoucherCommand
            {
                UploadField = request.UploadFileId,
                VoucherHeaderId = headerId,
                UploadedBy = request.UploadedBy,
                Vouchers = vouchers
            });

        _logger.LogInformation(
            "API Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}",
            "UploadVoucher",
            headerId);

        return Success(result, "Vouchers uploaded successfully");
    }
}