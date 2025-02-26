using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Domain.Companies;

namespace ReceivablesFactoring.Application.Abstractions;

public interface ICompanyService
{
    Task<string> Auth(string cnpj);
    Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto);
    Task<CompanyDto> GetCompanyAsync(Guid id);
}
