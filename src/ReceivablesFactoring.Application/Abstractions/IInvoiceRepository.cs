using ReceivablesFactoring.Domain.Invoices;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Application.Abstractions;

public interface IInvoiceRepository
{
    Task<List<Invoice>> GetByCompanyIdAsync(Guid companyId);
    Task<List<Invoice>> GetInCartByCompanyAsync(Guid companyId);
    Task<Invoice?> GetByCompanyIdAndNumber(Guid companyId, int number);
    Task<List<Invoice>> GetByCompanyIdAsync(Guid companyId, Expression<Func<Invoice, bool>> filter);
    void Add(Invoice invoice);
    void Update(Invoice invoice);
    Task<bool> AnyAsync(Expression<Func<Invoice, bool>> filter);
    Task<decimal> GetTotalAmountInCartAsync(Guid companyId);
}
