using ReceivablesFactoring.Domain.Companies;

namespace ReceivablesFactoring.Domain.Extensions;

public static class ProductCategoryExtensions
{
    private static readonly Dictionary<CompanyCategory, string> CategoryDescriptions = new()
    {
        { CompanyCategory.Services, "Serviços" },
        { CompanyCategory.Products, "Produtos" }
    };

    public static string GetEnumDescription(this CompanyCategory value)
    {
        return CategoryDescriptions.TryGetValue(value, out var description) ? description : value.ToString();
    }

    public static bool IsValidCompanyCategory(this string value)
    {
        return CategoryDescriptions.ContainsValue(value);
    }

    public static CompanyCategory ToCompanyCategory(this string value)
    {
        return CategoryDescriptions.FirstOrDefault(x => x.Value == value).Key;
    }
}
