using System.Text.Json.Serialization;

namespace ReceivablesFactoring.Application.Models;

public class FactoringDto
{
    [JsonPropertyName("empresa")]
    public string CompanyName { get; set; }

    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; }

    [JsonPropertyName("limite")]
    public decimal FactoringLimit { get; set; }

    [JsonPropertyName("notas_fiscais")]
    public List<FactoredInvoiceDto> FactoredInvoices { get; set; } = new();

    [JsonPropertyName("total_liquido")]
    public decimal TotalLiquidValue { get; set; }

    [JsonPropertyName("total_bruto")]
    public decimal TotalGrossValue { get; set; }
}
