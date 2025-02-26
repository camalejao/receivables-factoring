using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Domain.Invoices;
using ReceivablesFactoring.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Infrastructure.Repositories;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDateProvider _dateProvider;

    public InvoiceRepository(ApplicationDbContext applicationDbContext, IDateProvider dateProvider)
    {
        _dbContext = applicationDbContext;
        _dateProvider = dateProvider;
    }

    public async Task<List<Invoice>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _dbContext.Invoices.Where(x => x.CompanyId == companyId).ToListAsync();
    }

    public async Task<List<Invoice>> GetByCompanyIdAsync(Guid companyId, Expression<Func<Invoice, bool>> filter)
    {
        return await _dbContext.Invoices.Where(x => x.CompanyId == companyId).Where(filter).ToListAsync();
    }

    public void Add(Invoice invoice)
    {
        _dbContext.Invoices.Add(invoice);
    }

    public void Update(Invoice invoice)
    {
        _dbContext.Invoices.Update(invoice);
    }

    public async Task<bool> AnyAsync(Expression<Func<Invoice, bool>> filter)
    {
        return await _dbContext.Invoices.AnyAsync(filter);
    }

    public async Task<Invoice?> GetByCompanyIdAndNumber(Guid companyId, int number)
    {
        return await _dbContext.Invoices
            .Where(x => x.Number == number && x.CompanyId == companyId)
            .Include(x => x.Company)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Invoice>> GetInCartByCompanyAsync(Guid companyId)
    {
        return await _dbContext.Invoices
            .Where(x => x.CompanyId == companyId && x.InCart && x.DueDate >= _dateProvider.Today)
            .ToListAsync();
    }

    public Task<decimal> GetTotalAmountInCartAsync(Guid companyId)
    {
        return _dbContext.Invoices
            .Where(x => x.CompanyId == companyId && x.InCart && x.DueDate >= _dateProvider.Today)
            .SumAsync(x => x.Value);
    }
}
