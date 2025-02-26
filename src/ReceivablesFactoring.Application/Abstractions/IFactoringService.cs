using ReceivablesFactoring.Application.Models;

namespace ReceivablesFactoring.Application.Abstractions;

public interface IFactoringService
{
    Task<FactoringDto> GetFactoringAsync(Guid companyId);
    Task<InvoiceDto> AddInvoiceAsync(Guid companyId, InvoiceNumberDto invoice);
    Task<InvoiceDto> RemoveInvoiceAsync(Guid companyId, InvoiceNumberDto invoice);
}
