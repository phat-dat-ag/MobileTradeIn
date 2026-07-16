using MediatR;
using Microsoft.AspNetCore.Mvc;
using MobileTradeIn.Api.Models;
using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucherCsv;
using MobileTradeIn.Application.Interfaces.Services;

namespace MobileTradeIn.Api.Controllers;

[ApiController]
[Route("api/vouchers")]
public class VoucherController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICsvService _csvService;

    public VoucherController(IMediator mediator, ICsvService csvService)
    {
        _mediator = mediator;
        _csvService = csvService;
    }

    [HttpPost("header")]
    public async Task<IActionResult> CreateHeader(
       [FromBody] CreateVoucherHeaderCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("header/{headerId}/upload")]
    public async Task<IActionResult> UploadVoucher(
        int headerId,
        [FromForm] UploadVoucherRequest request)
    {
        using var stream = request.File.OpenReadStream();

        var vouchers = await _csvService.ReadVoucherCsvAsync(
            stream,
            headerId,
            request.UploadedBy);

        var result = await _mediator.Send(
            new UploadVoucherCsvCommand
            {
                VoucherHeaderId = headerId,
                UploadedBy = request.UploadedBy,
                Vouchers = vouchers
            });

        return Ok(result);
    }
}