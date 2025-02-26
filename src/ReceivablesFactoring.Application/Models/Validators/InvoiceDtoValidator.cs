using ReceivablesFactoring.Application.Abstractions;
using FluentValidation;

namespace ReceivablesFactoring.Application.Models.Validators;

public class InvoiceDtoValidator : AbstractValidator<InvoiceDto>
{
    private readonly IDateProvider _dateProvider;

    public InvoiceDtoValidator(IDateProvider dateProvider)
    {
        _dateProvider = dateProvider;

        RuleFor(x => x.Number)
            .GreaterThan(0)
            .WithMessage("Invoice number must be greater than 0");
        RuleFor(x => x.DueDate)
            .NotEmpty()
            .WithMessage("Due date must be informed");
        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(_dateProvider.Today)
            .WithMessage("Due date must be greater than or equal to today");
        RuleFor(x => x.Value)
            .GreaterThan(0)
            .WithMessage("Invoice value must be greater than 0");
    }
}
