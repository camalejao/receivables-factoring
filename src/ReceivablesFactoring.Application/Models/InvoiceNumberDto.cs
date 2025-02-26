
using System.Text.Json.Serialization;

namespace ReceivablesFactoring.Application.Models;

public record InvoiceNumberDto([property:JsonPropertyName("numero")] int Number);
