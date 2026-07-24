using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Api.Models;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucherCsv;
using MobileTradeIn.Application.Interfaces.Services;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/vouchers")]
public class VoucherController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<VoucherController> _logger;
    private readonly ICsvService _csvService;

    public VoucherController(IMediator mediator, ICsvService csvService, ILogger<VoucherController> logger)
    {
        _mediator = mediator;
        _csvService = csvService;
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
            "UploadVoucherCsv",
            headerId);

        using var stream = request.File.OpenReadStream();

        var vouchers = await _csvService.ReadVoucherCsvAsync(
            stream,
            headerId,
            request.UploadedBy);

        _logger.LogInformation(
            "API Step Completed. Step={Step}. VoucherHeaderId={VoucherHeaderId}. VoucherCount={VoucherCount}",
            "ReadVoucherCsv",
            headerId,
            vouchers.Count);

        var result = await _mediator.Send(
            new UploadVoucherCsvCommand
            {
                UploadField = request.UploadFileId,
                VoucherHeaderId = headerId,
                UploadedBy = request.UploadedBy,
                Vouchers = vouchers
            });

        _logger.LogInformation(
            "API Completed. Operation={Operation}. VoucherHeaderId={VoucherHeaderId}",
            "UploadVoucherCsv",
            headerId);

        return Success(result, "Vouchers uploaded successfully");
    }
}