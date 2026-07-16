using MobileTradeIn.Application.DTOs.Email;
using MobileTradeIn.Application.DTOs.TradeIn;

namespace MobileTradeIn.Application.Interfaces.Repositories;

public interface ITradeInRepository
{
    Task<CreateTradeInResponse> CreateTradeInAsync(CreateTradeInRequest request);

    Task ConfirmTradeInAsync(ConfirmTradeInRequest request);

    Task RejectTradeInAsync(RejectTradeInRequest request);

    Task<TradeInDto?> GetTradeInByIdAsync(int tradeInOfferId);

    Task<TradeInEmailDto?> GetTradeInEmailAsync(int tradeInOfferId);
}
