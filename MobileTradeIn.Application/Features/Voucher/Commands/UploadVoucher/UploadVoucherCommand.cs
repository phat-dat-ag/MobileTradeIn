using MediatR;
using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;

public class UploadVoucherCommand : IRequest<UploadVoucherResponse>
{
    public int UploadField { get; set; }

    public int VoucherHeaderId { get; set; }

    public string UploadedBy { get; set; } = string.Empty;

    public List<VoucherImportDto> Vouchers { get; set; } = [];
}