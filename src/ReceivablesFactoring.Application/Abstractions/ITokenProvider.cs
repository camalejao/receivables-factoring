using ReceivablesFactoring.Domain.Companies;

namespace ReceivablesFactoring.Application.Abstractions;

public interface ITokenProvider
{
    string CreateToken(Company company);
    Guid GetCompanyIdFromToken();
}
