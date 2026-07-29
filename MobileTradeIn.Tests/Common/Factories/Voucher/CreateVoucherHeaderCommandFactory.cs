using MobileTradeIn.Application.Features.Voucher.Commands.CreateVoucherHeader;

namespace MobileTradeIn.Tests.Common.Factories.Voucher
{
    public class CreateVoucherHeaderCommandFactory
    {
        public static CreateVoucherHeaderCommand CreateCreateVoucherHeaderCommand(
            string voucherBatchCode, int productId, decimal voucherValue, int quantity, string description, string createdBy)
        {
            return new CreateVoucherHeaderCommand
            {
                VoucherBatchCode = voucherBatchCode,
                ProductId = productId,
                VoucherValue = voucherValue,
                Quantity = quantity,
                Description = description,
                CreatedBy = createdBy
            };
        }
    }
}
