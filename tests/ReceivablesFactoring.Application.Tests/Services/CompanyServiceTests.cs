using Moq;
using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Application.Services;
using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Exceptions;
using FluentValidation;
using Xunit;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Application.Tests.Services;

public class CompanyServiceTests
{
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<CompanyDto>> _validatorMock;
    private readonly Mock<ITokenProvider> _tokenProviderMock;
    private readonly CompanyService _companyService;

    public CompanyServiceTests()
    {
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<CompanyDto>>();
        _tokenProviderMock = new Mock<ITokenProvider>();
        _companyService = new CompanyService(_companyRepositoryMock.Object, _validatorMock.Object, _unitOfWorkMock.Object, _tokenProviderMock.Object);
    }

    [Fact]
    public async Task Auth_ShouldReturnToken_WhenCompanyExists()
    {
        // Arrange
        var cnpj = "12345678000195";
        var company = new Company { Cnpj = cnpj };
        _companyRepositoryMock.Setup(repo => repo.GetByCnpjAsync(cnpj)).ReturnsAsync(company);
        _tokenProviderMock.Setup(provider => provider.CreateToken(company)).Returns("token");

        // Act
        var result = await _companyService.Auth(cnpj);

        // Assert
        Assert.Equal("token", result);
    }

    [Fact]
    public async Task Auth_ShouldThrowNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var cnpj = "12345678000195";
        _companyRepositoryMock.Setup(repo => repo.GetByCnpjAsync(cnpj)).ReturnsAsync((Company)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _companyService.Auth(cnpj));
    }

    [Fact]
    public async Task CreateCompanyAsync_ShouldCreateCompany_WhenValid()
    {
        // Arrange
        var companyDto = new CompanyDto { Cnpj = "12345678000195", Name = "Test Company", MonthlyBilling = 10000, Category = "Serviços" };
        _validatorMock.Setup(v => v.Validate(companyDto)).Returns(new FluentValidation.Results.ValidationResult());
        _companyRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Company, bool>>>())).ReturnsAsync(false);

        // Act
        var result = await _companyService.CreateCompanyAsync(companyDto);

        // Assert
        Assert.Equal(companyDto, result);
        _companyRepositoryMock.Verify(repo => repo.Add(It.IsAny<Company>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
    }

    [Fact]
    public async Task CreateCompanyAsync_ShouldThrowValidationFailureException_WhenCompanyAlreadyExists()
    {
        // Arrange
        var companyDto = new CompanyDto { Cnpj = "12345678000195", Name = "Test Company", MonthlyBilling = 10000, Category = "Serviços" };
        _validatorMock.Setup(v => v.Validate(companyDto)).Returns(new FluentValidation.Results.ValidationResult());
        _companyRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Company, bool>>>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailureException>(() => _companyService.CreateCompanyAsync(companyDto));
    }
}


