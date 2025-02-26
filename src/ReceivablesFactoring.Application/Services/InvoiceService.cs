using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Domain.Invoices;
using FluentValidation;
using ReceivablesFactoring.Domain.Exceptions;

namespace ReceivablesFactoring.Application.Services;

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<InvoiceDto> _validator;

    public InvoiceService(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork, IValidator<InvoiceDto> validator)
    {
        _invoiceRepository = invoiceRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<InvoiceDto> CreateInvoiceAsync(Guid companyId, InvoiceDto invoiceDto)
    {
        var validationResult = _validator.Validate(invoiceDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationFailureException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        if (await _invoiceRepository.AnyAsync(x => x.CompanyId == companyId && x.Number == invoiceDto.Number))
        {
            throw new ValidationFailureException($"Invoice with number {invoiceDto.Number} already exists in this company.");
        }

        var invoice = new Invoice
        {
            CompanyId = companyId,
            Number = invoiceDto.Number,
            Value = invoiceDto.Value,
            DueDate = invoiceDto.DueDate,
        };

        _invoiceRepository.Add(invoice);
        await _unitOfWork.Commit();

        return invoiceDto;
    }

    public async Task<List<InvoiceDto>> GetInvoicesAsync(Guid companyId)
    {
        var invoices = await _invoiceRepository.GetByCompanyIdAsync(companyId);

        return invoices.Select(i => new InvoiceDto
        {
            Number = i.Number,
            Value = i.Value,
            DueDate = i.DueDate,
        }).ToList();
    }
}
