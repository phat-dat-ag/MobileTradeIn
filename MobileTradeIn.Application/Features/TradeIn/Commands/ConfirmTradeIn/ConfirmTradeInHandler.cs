using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.TradeIn;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Application.Interfaces.Services;
using System.Diagnostics;

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
            "Business Started. Operation={Operation}. TradeInOfferId={TradeInOfferId}",
            "ConfirmTradeIn",
            request.TradeInOfferId);

        var stopwatch = Stopwatch.StartNew();

        await _repository.ConfirmTradeInAsync(
            new ConfirmTradeInRequest
            {
                TradeInOfferId = request.TradeInOfferId,
                ConfirmedBy = request.ConfirmedBy,
                Notes = request.Notes
            });

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}",
            "ConfirmTradeInDatabase",
            request.TradeInOfferId);

        var emailInfo =
            await _repository.GetTradeInEmailAsync(request.TradeInOfferId);

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}",
            "LoadTradeInEmail",
            request.TradeInOfferId);

        if (emailInfo == null)
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. TradeInOfferId={TradeInOfferId}",
                "LoadTradeInEmail",
                request.TradeInOfferId);

            throw new EmailInforNotFoundException();
        }

        var template =
            await _emailTemplateRepository.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED");

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TemplateCode={TemplateCode}",
            "LoadEmailTemplate",
            "TRADEIN_APPROVED");

        if (template == null)
        {
            _logger.LogWarning(
                 "Business Failed. Step={Step}. TemplateCode={TemplateCode}",
                 "LoadEmailTemplate",
                 "TRADEIN_APPROVED");

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
            _logger.LogInformation(
                "Business Step Started. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                request.TradeInOfferId,
                emailInfo.CustomerEmail);

            await _emailService.SendEmailAsync(
                emailInfo.CustomerEmail,
                subject,
                body);

            _logger.LogInformation(
                "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                request.TradeInOfferId,
                emailInfo.CustomerEmail);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Business Failed. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                request.TradeInOfferId,
                emailInfo.CustomerEmail);
        }

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}. Elapsed={ElapsedMilliseconds}ms",
            "ConfirmTradeIn",
            request.TradeInOfferId,
            stopwatch.ElapsedMilliseconds);
    }
}