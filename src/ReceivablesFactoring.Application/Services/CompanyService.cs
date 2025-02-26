using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Extensions;
using FluentValidation;
using ReceivablesFactoring.Domain.Exceptions;

namespace ReceivablesFactoring.Application.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CompanyDto> _validator;
    private readonly ITokenProvider _tokenProvider;

    public CompanyService(ICompanyRepository companyRepository, IValidator<CompanyDto> validator, IUnitOfWork unitOfWork, ITokenProvider tokenProvider)
    {
        _companyRepository = companyRepository;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _tokenProvider = tokenProvider;
    }

    public async Task<string> Auth(string cnpj)
    {
        var company = await _companyRepository.GetByCnpjAsync(cnpj);
        if (company is null)
        {
            throw new NotFoundException($"Company {cnpj} not found");
        }

        return _tokenProvider.CreateToken(company);
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyDto companyDto)
    {
        var validationResult = _validator.Validate(companyDto);

        if (!validationResult.IsValid)
        {
            throw new ValidationFailureException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        if (await _companyRepository.AnyAsync(x => x.Cnpj == companyDto.Cnpj))
        {
            throw new ValidationFailureException($"Company {companyDto.Cnpj} already exists!");
        }

        var company = new Company
        {
            Cnpj = companyDto.Cnpj,
            Name = companyDto.Name,
            MonthlyBilling = companyDto.MonthlyBilling,
            Category = companyDto.Category.ToCompanyCategory(),
        };

        _companyRepository.Add(company);
        await _unitOfWork.Commit();

        return companyDto;
    }
}
