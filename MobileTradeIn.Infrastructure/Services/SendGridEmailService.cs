using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MobileTradeIn.Infrastructure.Services;

public class SendGridEmailService : IEmailService
{
    private readonly SendGridOptions _options;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(
        IOptions<SendGridOptions> options,
        ILogger<SendGridEmailService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string htmlContent)
    {
        var client = new SendGridClient(_options.ApiKey);

        var from = new EmailAddress(_options.FromEmail, _options.FromName);

        var to = new EmailAddress(toEmail);

        var message = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            plainTextContent: null,
            htmlContent: htmlContent);

        var response = await client.SendEmailAsync(message);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Body.ReadAsStringAsync();

            _logger.LogError(
                "Failed to send email. Status={StatusCode}, Error={Error}",
                response.StatusCode,
                error);

            throw new Exception("Failed to send email.");
        }

        _logger.LogInformation(
            "Email sent successfully to {Email}",
            toEmail);
    }
}