using MediatR;
using MobileTradeIn.Application.DTOs.Voucher;
using System.ComponentModel.DataAnnotations;

namespace MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;

public class CreateVoucherHeaderCommand : IRequest<CreateVoucherHeaderResponse>
{

    [Range(1, int.MaxValue)]
    public int UploadFileId { get; set; }

    [Required]
    [StringLength(100)]
    public string VoucherBatchCode { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal VoucherValue { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    public string CreatedBy { get; set; } = string.Empty;
}