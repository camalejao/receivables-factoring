using System.Text.Json.Serialization;

namespace ReceivablesFactoring.Application.Models;

public class FactoredInvoiceDto
{
    [JsonPropertyName("numero")]
    public int Number { get; set; }

    [JsonPropertyName("valor_bruto")]
    public decimal GrossValue { get; set; }

    [JsonPropertyName("valor_liquido")]
    public decimal LiquidValue { get; set; }
}
