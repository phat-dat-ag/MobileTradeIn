using MobileTradeIn.Application.Features.TradeIn.Commands.CreateTradeIn;

namespace MobileTradeIn.Tests.Common.Factories.TradeIn
{
    public class CreateTradeInCommandFactory
    {
        public static CreateTradeInCommand CreateCreateTradeInCommand(
            int customerId, int productId, string deviceCondition, string imei, string voucherCode, string createdBy)
        {
            return new CreateTradeInCommand
            {
                CustomerId = customerId,
                ProductId = productId,
                DeviceCondition = deviceCondition,
                IMEI = imei,
                VoucherCode = voucherCode,
                CreatedBy = createdBy
            };
        }
    }
}
