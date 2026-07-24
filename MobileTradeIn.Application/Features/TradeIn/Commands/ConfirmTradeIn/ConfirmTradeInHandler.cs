using MediatR;
using Microsoft.Extensions.Logging;
using MobileTradeIn.Application.Common.Exceptions.NotFound;
using MobileTradeIn.Application.DTOs.Email;
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

    private async Task<(TradeInEmailDto emailInfo, EmailTemplateDto emailTemplate)> GetEmailInforAndEmailTemplate(int tradeInOfferId)
    {
        var emailInfo =
            await _repository.GetTradeInEmailAsync(tradeInOfferId);

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}",
            "LoadTradeInEmail",
            tradeInOfferId);

        if (emailInfo == null)
        {
            _logger.LogWarning(
                "Business Failed. Step={Step}. TradeInOfferId={TradeInOfferId}",
                "LoadTradeInEmail",
                tradeInOfferId);

            throw new EmailInforNotFoundException();
        }

        var emailTemplate =
            await _emailTemplateRepository.GetEmailTemplateByTemplateCodeAsync("TRADEIN_APPROVED");

        _logger.LogInformation(
            "Business Step Completed. Step={Step}. TemplateCode={TemplateCode}",
            "LoadEmailTemplate",
            "TRADEIN_APPROVED");

        if (emailTemplate == null)
        {
            _logger.LogWarning(
                 "Business Failed. Step={Step}. TemplateCode={TemplateCode}",
                 "LoadEmailTemplate",
                 "TRADEIN_APPROVED");

            throw new EmailTemplateNotFoundException();
        }

        return (emailInfo, emailTemplate);
    }

    private (string subject, string body) BuildEmailContent(EmailTemplateDto template, TradeInEmailDto emailInfo)
    {
        var values = new Dictionary<string, string>
        {
            { "CustomerName", emailInfo.CustomerName },
            { "ProductName", emailInfo.ProductName },
            { "OfferAmount", emailInfo.OfferAmount.ToString("N0") + " VND" },
            { "TransactionNumber", emailInfo.TransactionNumber }
        };

        return
            (
            _emailTemplateService.RenderContentFromEmailTemplate(
                template.Subject,
                values),

            _emailTemplateService.RenderContentFromEmailTemplate(
                template.Content,
                values)
            );
    }

    private async Task SendConfirmationEmailAsync(int tradeInOfferId, string customerEmail, string subject, string body)
    {
        try
        {
            _logger.LogInformation(
                "Business Step Started. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                tradeInOfferId,
                customerEmail);

            await _emailService.SendEmailAsync(
                customerEmail,
                subject,
                body);

            _logger.LogInformation(
                "Business Step Completed. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                tradeInOfferId,
                customerEmail);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Business Failed. Step={Step}. TradeInOfferId={TradeInOfferId}. CustomerEmail={CustomerEmail}",
                "SendConfirmationEmail",
                tradeInOfferId,
                customerEmail);
        }
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

        var (emailInfo, emailTemplate) = await GetEmailInforAndEmailTemplate(request.TradeInOfferId);

        var (subject, body) = BuildEmailContent(emailTemplate, emailInfo);

        await SendConfirmationEmailAsync(request.TradeInOfferId, emailInfo.CustomerEmail, subject, body);

        stopwatch.Stop();

        _logger.LogInformation(
            "Business Completed. Operation={Operation}. TradeInOfferId={TradeInOfferId}. Elapsed={ElapsedMilliseconds}ms",
            "ConfirmTradeIn",
            request.TradeInOfferId,
            stopwatch.ElapsedMilliseconds);
    }
}