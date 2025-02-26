using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Invoices;
using Microsoft.EntityFrameworkCore;

namespace ReceivablesFactoring.Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

}
