using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ReceivablesFactoring.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly, includeInternalTypes: true);
        
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<IFactoringService, FactoringService>();

        return services;
    }
}
