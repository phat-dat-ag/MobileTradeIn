using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MobileTradeIn.Application.Interfaces.Repositories;
using MobileTradeIn.Application.Interfaces.Services;
using MobileTradeIn.Infrastructure.Repositories;
using MobileTradeIn.Infrastructure.Services;

namespace MobileTradeIn.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<DapperContext>();

        services.Configure<SendGridOptions>(configuration.GetSection("SendGrid"));

        services.AddScoped<ITradeInRepository, TradeInRepository>();

        services.AddScoped<IVoucherRepository, VoucherRepository>();

        services.AddScoped<ICsvService, CsvService>();

        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();

        services.AddScoped<IEmailTemplateService, EmailTemplateService>();

        services.AddScoped<IEmailService, SendGridEmailService>();

        return services;
    }
}