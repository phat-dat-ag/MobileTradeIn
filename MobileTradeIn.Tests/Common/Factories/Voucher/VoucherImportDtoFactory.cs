using MobileTradeIn.Application.DTOs.Voucher;

namespace MobileTradeIn.Tests.Common.Factories.Voucher
{
    public class VoucherImportDtoFactory
    {
        public static VoucherImportDto CreateVoucherImportDto(string voucherCode)
        {
            return new VoucherImportDto
            {
                VoucherCode = voucherCode,
                VoucherHeaderId = 1,
                StartDate = DateOnly.FromDateTime(DateTime.Today),
                EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(1)),
                IsActive = true,
                CreatedBy = "admin"
            };
        }
    }
}
