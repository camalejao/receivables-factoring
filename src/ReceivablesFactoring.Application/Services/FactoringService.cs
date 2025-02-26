using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Application.Models;
using ReceivablesFactoring.Domain.Exceptions;
using ReceivablesFactoring.Domain.Extensions;
using ReceivablesFactoring.Domain.Invoices;
using System.Text;

namespace ReceivablesFactoring.Application.Services;

public class FactoringService : IFactoringService
{
    private const double _factoringFee = 0.0465;

    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateProvider _dateProvider;

    public FactoringService(
        IInvoiceRepository invoiceRepository,
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        IDateProvider dateProvider)
    {
        _invoiceRepository = invoiceRepository;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _dateProvider = dateProvider;
    }

    public async Task<FactoringDto> GetFactoringAsync(Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        var invoices = await _invoiceRepository.GetInCartByCompanyAsync(companyId);

        if (company is null)
        {
            throw new NotFoundException("Company not found");
        }

        var factoredInvoices = invoices.Select(x => new FactoredInvoiceDto
        {
            Number = x.Number,
            GrossValue = x.Value.Round(),
            LiquidValue = CalculateFactoring(x).Round(),
        }).ToList();

        var grossTotal = invoices.Sum(x => x.Value).Round();
        var liquidTotal = factoredInvoices.Sum(x => x.LiquidValue).Round();

        return new FactoringDto
        {
            CompanyName = company.Name,
            Cnpj = company.Cnpj,
            FactoringLimit = company.CalculateFactoringLimit().Round(),
            FactoredInvoices = factoredInvoices.ToList(),
            TotalGrossValue = grossTotal,
            TotalLiquidValue =liquidTotal,
        };
    }

    public async Task<InvoiceDto> AddInvoiceAsync(Guid companyId, InvoiceNumberDto invoiceDto)
    {
        var invoice = await FindInvoice(companyId, invoiceDto);

        var limit = invoice.Company.CalculateFactoringLimit();
        var currentTotal = await _invoiceRepository.GetTotalAmountInCartAsync(companyId);

        if (invoice.Value + currentTotal > limit)
        {
            throw new ValidationFailureException($"Adding this Invoice will exceed the factoring limit of {limit}. The current total is {currentTotal}.");
        }

        return await UpdateInvoiceInCart(invoice, true);
    }

    public async Task<InvoiceDto> RemoveInvoiceAsync(Guid companyId, InvoiceNumberDto invoiceDto)
    {
        var invoice = await FindInvoice(companyId, invoiceDto);
        return await UpdateInvoiceInCart(invoice, false);
    }


    private decimal CalculateFactoring(Invoice x)
    {
        var timeDiff = x.DueDate.ToDateTime(TimeOnly.MinValue) - _dateProvider.Today.ToDateTime(TimeOnly.MinValue);

        return x.Value / (decimal)Math.Pow(1 + _factoringFee, timeDiff.Days / 30.0);
    }

    private async Task<Invoice> FindInvoice(Guid companyId, InvoiceNumberDto invoiceDto)
    {
        var invoice = await _invoiceRepository.GetByCompanyIdAndNumber(companyId, invoiceDto.Number);
        if (invoice is null)
        {
            throw new NotFoundException($"Invoice {invoiceDto.Number} not found");
        }

        return invoice;
    }

    private async Task<InvoiceDto> UpdateInvoiceInCart(Invoice invoice, bool inCart)
    {
        invoice.InCart = inCart;

        _invoiceRepository.Update(invoice);
        await _unitOfWork.Commit();

        return new InvoiceDto
        {
            Number = invoice.Number,
            Value = invoice.Value,
            DueDate = invoice.DueDate,
        };
    }
}
