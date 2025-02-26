using ReceivablesFactoring.Domain.Invoices;

namespace ReceivablesFactoring.Domain.Companies;

public class Company
{
    public Guid Id { get; set; }

    public string Cnpj { get; set; }

    public string Name { get; set; }

    public decimal MonthlyBilling { get; set; }

    public CompanyCategory Category { get; set; }

    public ICollection<Invoice> Invoices { get; private set; }


    private const decimal MinBillingLimit = 10000m;
    private const decimal MidBillingLimit = 50000m;
    private const decimal MaxBillingLimit = 100000m;

    private static readonly Dictionary<(decimal Min, decimal Max, CompanyCategory Category), decimal> FactoringLimitPercentages =
        new()
        {
            { (MinBillingLimit, MidBillingLimit, CompanyCategory.Services), 0.5m },
            { (MinBillingLimit, MidBillingLimit, CompanyCategory.Products), 0.5m },
            { (MidBillingLimit + 1, MaxBillingLimit, CompanyCategory.Services), 0.55m },
            { (MidBillingLimit + 1, MaxBillingLimit, CompanyCategory.Products), 0.6m },
            { (MaxBillingLimit + 1, decimal.MaxValue, CompanyCategory.Services), 0.6m },
            { (MaxBillingLimit + 1, decimal.MaxValue, CompanyCategory.Products), 0.65m }
        };

    public decimal CalculateFactoringLimit()
    {
        foreach (var key in FactoringLimitPercentages.Keys)
        {
            if (MonthlyBilling >= key.Min && MonthlyBilling <= key.Max && Category == key.Category)
            {
                return MonthlyBilling * FactoringLimitPercentages[key];
            }
        }

        return 0m;
    }
}
