using ReceivablesFactoring.Domain.Companies;
using Xunit;

namespace ReceivablesFactoring.Domain.Tests.Companies;

public class CompanyTests
{
    [Theory]
    [InlineData(10000, CompanyCategory.Services, 5000)]
    [InlineData(60000, CompanyCategory.Services, 33000)]
    [InlineData(60000, CompanyCategory.Products, 36000)]
    [InlineData(150000, CompanyCategory.Services, 90000)]
    [InlineData(150000, CompanyCategory.Products, 97500)]
    public void CalculateFactoringLimit_ShouldCalculateCorrectly(decimal monthlyBilling, CompanyCategory category, decimal expected)
    {
        var company = new Company
        {
            MonthlyBilling = monthlyBilling,
            Category = category
        };

        var result = company.CalculateFactoringLimit();
        Assert.Equal(expected, result);
    }
}
