using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Infrastructure.Authentication;
using ReceivablesFactoring.Infrastructure.Database;
using ReceivablesFactoring.Infrastructure.Repositories;
using ReceivablesFactoring.Infrastructure.Time;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ReceivablesFactoring.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRepositories();
        services.AddAuth(configuration);
        services.AddDateProvider();        
        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = $"Server={configuration["DB:Server"]};Initial Catalog={configuration["DB:Catalog"]};User ID={configuration["DB:User"]};Password={configuration["DB:Password"]};Encrypt=false";

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"]!)),
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddHttpContextAccessor();
        services.AddSingleton<ITokenProvider, TokenProvider>();
        return services;
    }

    private static IServiceCollection AddDateProvider(this IServiceCollection services)
    {
        services.AddSingleton<IDateProvider, DateProvider>();
        return services;
    }
}
