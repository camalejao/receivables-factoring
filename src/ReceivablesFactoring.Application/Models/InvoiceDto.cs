using System.Text.Json.Serialization;

namespace ReceivablesFactoring.Application.Models;

public class InvoiceDto
{
    [JsonPropertyName("numero")]
    public int Number { get; set; }

    [JsonPropertyName("valor")]
    public decimal Value { get; set; }
    
    [JsonPropertyName("data_vencimento")]
    public DateOnly DueDate { get; set; }
}
