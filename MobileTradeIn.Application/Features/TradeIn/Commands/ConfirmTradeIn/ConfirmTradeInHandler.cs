using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Application.Interfaces.Services;

namespace MobileTradeIn.Application.Features.TradeIn.Commands.ConfirmTradeIn;

public class ConfirmTradeInHandler : IRequestHandler<ConfirmTradeInCommand>
{
    private readonly ITradeInRepository _repository;
    private readonly ILogger<ConfirmTradeInHandler> _logger;
    private readonly IEmailTemplateRepository _emailTemplateRepository;
    private readonly IEmailTemplateService _emailTemplateService;
    private readonly IEmailService _emailService;

    public ConfirmTradeInHandler(
        ITradeInRepository repository,
        ILogger<ConfirmTradeInHandler> logger,
        IEmailTemplateRepository emailTemplateRepository,
        IEmailTemplateService emailTemplateService,
        IEmailService emailService
        )
    {
        _repository = repository;
        _logger = logger;
        _emailTemplateRepository = emailTemplateRepository;
        _emailTemplateService = emailTemplateService;
        _emailService = emailService;
    }

    public async Task Handle(
        ConfirmTradeInCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
           "Start confirming TradeIn. TradeInOfferId={TradeInOfferId}",
           request.TradeInOfferId);

        await _repository.ConfirmTradeInAsync(
            new ConfirmTradeInRequest
            {
                TradeInOfferId = request.TradeInOfferId,
                ConfirmedBy = request.ConfirmedBy,
                Notes = request.Notes
            });

        var emailInfo =
            await _repository.GetTradeInEmailAsync(request.TradeInOfferId);

        if (emailInfo == null)
        {
            _logger.LogWarning(
                "TradeIn email information not found. OfferId={OfferId}",
                request.TradeInOfferId);

            throw new EmailInforNotFoundException();
        }

        var template =
            await _emailTemplateRepository.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED");

        if (template == null)
        {
            _logger.LogWarning(
                "Email template TRADEIN_APPROVED not found.");

            throw new EmailTemplateNotFoundException();
        }

        var values = new Dictionary<string, string>
        {
            { "CustomerName", emailInfo.CustomerName },
            { "ProductName", emailInfo.ProductName },
            { "OfferAmount", emailInfo.OfferAmount.ToString("N0") + " VND" },
            { "TransactionNumber", emailInfo.TransactionNumber }
        };

        var subject =
            _emailTemplateService.RenderContentFromEmailTemplate(
                template.Subject,
                values);

        var body =
            _emailTemplateService.RenderContentFromEmailTemplate(
                template.Content,
                values);

        try
        {
            await _emailService.SendEmailAsync(
                emailInfo.CustomerEmail,
                subject,
                body);

            _logger.LogInformation(
                "Confirmation email sent to {Email}",
                emailInfo.CustomerEmail);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to send confirmation email to {Email}",
                emailInfo.CustomerEmail);
        }

        _logger.LogInformation(
           "Tradein confirmed successfully. TradeInOfferId={TradeInOfferId}",
           request.TradeInOfferId);
    }
}