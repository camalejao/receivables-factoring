
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Domain.Invoices;

namespace ReceivablesFactoring.Application.Abstractions;

public interface IInvoiceService
{
    Task<InvoiceDto> CreateInvoiceAsync(Guid companyId, InvoiceDto invoiceDto);
    Task<List<InvoiceDto>> GetInvoicesAsync(Guid companyId);
}
