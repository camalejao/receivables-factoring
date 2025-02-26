using Moq;
using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Application.Services;
using ReceivablesFactoring.Domain.Companies;
using ReceivablesFactoring.Domain.Invoices;
using ReceivablesFactoring.Domain.Exceptions;
using Xunit;

namespace ReceivablesFactoring.Application.Tests.Services;

public class FactoringServiceTests
{
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private readonly Mock<ICompanyRepository> _companyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateProvider> _dateProviderMock;
    private readonly FactoringService _factoringService;

    public FactoringServiceTests()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _companyRepositoryMock = new Mock<ICompanyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dateProviderMock = new Mock<IDateProvider>();
        _factoringService = new FactoringService(_invoiceRepositoryMock.Object, _companyRepositoryMock.Object, _unitOfWorkMock.Object, _dateProviderMock.Object);
    }

    [Fact]
    public async Task GetFactoringAsync_ShouldReturnFactoringDto_WhenCompanyExists()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var company = new Company { Id = companyId, Name = "Test Company", Cnpj = "12345678000195", MonthlyBilling = 100000, Category = CompanyCategory.Services };
        var invoices = new List<Invoice>
        {
            new Invoice { Number = 1, Value = 1000m, DueDate = new DateOnly(2025, 12, 31), Company = company },
            new Invoice { Number = 2, Value = 2000m, DueDate = new DateOnly(2025, 12, 31), Company = company }
        };
        _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(companyId)).ReturnsAsync(company);
        _invoiceRepositoryMock.Setup(repo => repo.GetInCartByCompanyAsync(companyId)).ReturnsAsync(invoices);
        _dateProviderMock.Setup(dp => dp.Today).Returns(new DateOnly(2025, 12, 1));

        // Act
        var result = await _factoringService.GetFactoringAsync(companyId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Company", result.CompanyName);
        Assert.Equal("12345678000195", result.Cnpj);
        Assert.Equal(2, result.FactoredInvoices.Count);
    }

    [Fact]
    public async Task GetFactoringAsync_ShouldThrowNotFoundException_WhenCompanyDoesNotExist()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        _companyRepositoryMock.Setup(repo => repo.GetByIdAsync(companyId)).ReturnsAsync((Company)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _factoringService.GetFactoringAsync(companyId));
    }

    [Fact]
    public async Task AddInvoiceAsync_ShouldAddInvoiceToCart_WhenValid()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var invoiceDto = new InvoiceNumberDto(123);
        var company = new Company { Id = companyId, MonthlyBilling = 100000, Category = CompanyCategory.Services };
        var invoice = new Invoice { CompanyId = companyId, Number = 123, Value = 1000, Company = company };
        _invoiceRepositoryMock.Setup(repo => repo.GetByCompanyIdAndNumber(companyId, invoiceDto.Number)).ReturnsAsync(invoice);
        _invoiceRepositoryMock.Setup(repo => repo.GetTotalAmountInCartAsync(companyId)).ReturnsAsync(0);

        // Act
        var result = await _factoringService.AddInvoiceAsync(companyId, invoiceDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(123, result.Number);
        _invoiceRepositoryMock.Verify(repo => repo.Update(It.IsAny<Invoice>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
    }

    [Fact]
    public async Task AddInvoiceAsync_ShouldThrowValidationFailureException_WhenExceedsFactoringLimit()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var invoiceDto = new InvoiceNumberDto(123);
        var company = new Company { Id = companyId, MonthlyBilling = 100000, Category = CompanyCategory.Services };
        var invoice = new Invoice { CompanyId = companyId, Number = 123, Value = 100000, Company = company };
        _invoiceRepositoryMock.Setup(repo => repo.GetByCompanyIdAndNumber(companyId, invoiceDto.Number)).ReturnsAsync(invoice);
        _invoiceRepositoryMock.Setup(repo => repo.GetTotalAmountInCartAsync(companyId)).ReturnsAsync(100000);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailureException>(() => _factoringService.AddInvoiceAsync(companyId, invoiceDto));
    }
}


