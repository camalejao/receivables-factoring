using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Extensions;

namespace ReceivablesFactoring.Domain.Tests.Extensions;

public class ProductCategoryExtensionsTest
{
    [Theory]
    [InlineData(CompanyCategory.Services, "Serviços")]
    [InlineData(CompanyCategory.Products, "Produtos")]
    [InlineData((CompanyCategory)999, "999")]
    public void GetEnumDescription_ShouldReturnCorrectDescription(CompanyCategory category, string expectedDescription)
    {
        var result = category.GetEnumDescription();
        Assert.Equal(expectedDescription, result);
    }

    [Theory]
    [InlineData("Serviços", true)]
    [InlineData("Produtos", true)]
    [InlineData("InvalidCategory", false)]
    public void IsValidCompanyCategory_ShouldValidateCorrectly(string category, bool expected)
    {
        var result = category.IsValidCompanyCategory();
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("Serviços", CompanyCategory.Services)]
    [InlineData("Produtos", CompanyCategory.Products)]
    public void ToCompanyCategory_ShouldConvertCorrectly(string category, CompanyCategory expected)
    {
        var result = category.ToCompanyCategory();
        Assert.Equal(expected, result);
    }
}
