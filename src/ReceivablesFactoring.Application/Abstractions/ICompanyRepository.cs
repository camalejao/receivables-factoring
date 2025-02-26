using ReceivablesFactoring.Domain.Companies;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Application.Abstractions;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    void Add(Company company);
    void Update(Company company);
    Task<bool> AnyAsync(Expression<Func<Company, bool>> predicate);
    Task<Company?> GetByCnpjAsync(string cnpj);
}
