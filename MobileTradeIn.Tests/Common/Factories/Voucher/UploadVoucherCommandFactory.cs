using MobileTradeIn.Application.DTOs.Voucher;
using MobileTradeIn.Application.Features.Voucher.Commands.UploadVoucher;

namespace MobileTradeIn.Tests.Common.Factories.Voucher
{
    public class UploadVoucherCommandFactory
    {
        public static UploadVoucherCommand CreateUploadVoucherCommand(List<VoucherImportDto> vouchers)
        {
            return new UploadVoucherCommand
            {
                VoucherHeaderId = 1,
                UploadedBy = "admin",
                Vouchers = vouchers,
            };
        }

        public static UploadVoucherCommand CreateUploadVoucherCommand(int voucherHeaderId, string uploadedBy)
        {
            return new UploadVoucherCommand
            {
                VoucherHeaderId = voucherHeaderId,
                UploadedBy = uploadedBy,
                Vouchers = [
                    new VoucherImportDto()
                ],
            };
        }
    }
}
