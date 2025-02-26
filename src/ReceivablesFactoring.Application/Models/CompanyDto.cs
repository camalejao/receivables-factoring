using System.Text.Json.Serialization;

namespace ReceivablesFactoring.Application.Models;

public class CompanyDto
{
    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; }

    [JsonPropertyName("nome")]
    public string Name { get; set; }

    [JsonPropertyName("faturamento_mensal")]
    public decimal MonthlyBilling { get; set; }

    [JsonPropertyName("ramo")]
    public string Category { get; set; }
}
