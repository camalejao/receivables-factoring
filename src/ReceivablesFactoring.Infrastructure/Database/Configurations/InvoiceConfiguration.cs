using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReceivablesFactoring.Infrastructure.Database.Configurations;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Company)
            .WithMany(x => x.Invoices)
            .HasForeignKey(x => x.CompanyId);

        builder.HasIndex(x => new { x.CompanyId, x.Number }).IsUnique();

        builder.Property(x => x.CompanyId)
            .IsRequired();

        builder.Property(x => x.Number)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired();

        builder.Property(x => x.DueDate)
            .IsRequired();
    }
}
