using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CompanyRepository(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Companies.FindAsync(id);
    }

    public void Add(Company company)
    {
        _dbContext.Companies.Add(company);
    }

    public void Update(Company company)
    {
        _dbContext.Companies.Update(company);
    }

    public async Task<bool> AnyAsync(Expression<Func<Company, bool>> predicate)
    {
        return await _dbContext.Companies.AnyAsync(predicate);
    }

    public async Task<Company?> GetByCnpjAsync(string cnpj)
    {
        return await _dbContext.Companies.Where(x => x.Cnpj == cnpj).FirstOrDefaultAsync();
    }
}
