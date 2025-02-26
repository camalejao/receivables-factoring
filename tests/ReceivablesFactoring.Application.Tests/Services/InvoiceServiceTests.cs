using Moq;
using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Application.Services;
using ReceivablesFactoring.Domain.Invoices;
using ReceivablesFactoring.Domain.Exceptions;
using FluentValidation;
using Xunit;
using System.Linq.Expressions;

namespace ReceivablesFactoring.Application.Tests.Services;

public class InvoiceServiceTests
{
    private readonly Mock<IInvoiceRepository> _invoiceRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IValidator<InvoiceDto>> _validatorMock;
    private readonly InvoiceService _invoiceService;

    public InvoiceServiceTests()
    {
        _invoiceRepositoryMock = new Mock<IInvoiceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _validatorMock = new Mock<IValidator<InvoiceDto>>();
        _invoiceService = new InvoiceService(_invoiceRepositoryMock.Object, _unitOfWorkMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task CreateInvoiceAsync_ShouldCreateInvoice_WhenValid()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var invoiceDto = new InvoiceDto { Number = 123, Value = 1000, DueDate = new DateOnly(2025, 12, 31) };
        _validatorMock.Setup(v => v.Validate(invoiceDto)).Returns(new FluentValidation.Results.ValidationResult());
        _invoiceRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Invoice, bool>>>())).ReturnsAsync(false);

        // Act
        var result = await _invoiceService.CreateInvoiceAsync(companyId, invoiceDto);

        // Assert
        Assert.Equal(invoiceDto, result);
        _invoiceRepositoryMock.Verify(repo => repo.Add(It.IsAny<Invoice>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.Commit(), Times.Once);
    }

    [Fact]
    public async Task CreateInvoiceAsync_ShouldThrowValidationFailureException_WhenInvoiceAlreadyExists()
    {
        // Arrange
        var companyId = Guid.NewGuid();
        var invoiceDto = new InvoiceDto { Number = 123, Value = 1000, DueDate = new DateOnly(2025, 12, 31) };
        _validatorMock.Setup(v => v.Validate(invoiceDto)).Returns(new FluentValidation.Results.ValidationResult());
        _invoiceRepositoryMock.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Invoice, bool>>>())).ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationFailureException>(() => _invoiceService.CreateInvoiceAsync(companyId, invoiceDto));
    }
}


