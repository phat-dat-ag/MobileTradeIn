using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Tests.Common.Factories.Voucher
{
    public class VoucherHeaderDtoFactory
    {
        public static VoucherHeaderDto CreateVoucherHeaderDto(int voucherHeaderId, int quantity)
        {
            return new VoucherHeaderDto
            {
                VoucherHeaderId = voucherHeaderId,
                Quantity = quantity
            };
        }
    }
}
