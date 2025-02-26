using ReceivablesFactoring.Domain.Companies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ReceivablesFactoring.Infrastructure.Database.Configurations;

internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Cnpj).IsUnique();

        builder.Property(x => x.Cnpj)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.MonthlyBilling)
            .IsRequired();

        builder.Property(x => x.Category)
            .IsRequired();

        builder.HasMany(x => x.Invoices)
            .WithOne(x => x.Company)
            .HasForeignKey(x => x.CompanyId);
    }
}
