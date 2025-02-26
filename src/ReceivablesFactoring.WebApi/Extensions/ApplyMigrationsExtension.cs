using ReceivablesFactoring.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ReceivablesFactoring.WebApi.Extensions;

public static class ApplyMigrationsExtension
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();
    }
}

