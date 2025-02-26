using ReceivablesFactoring.Domain.Extensions;
using FluentValidation;

namespace ReceivablesFactoring.Application.Models.Validators;

public class CompanyDtoValidator : AbstractValidator<CompanyDto>
{
    public CompanyDtoValidator()
    {
        RuleFor(x => x.Cnpj)
            .Must(x => x.IsValidCnpj())
            .WithMessage("A valid CNPJ must be informed without punctuation");
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name must be informed and have at most 100 characters");
        RuleFor(x => x.MonthlyBilling)
            .GreaterThan(0)
            .WithMessage("Monthly Billing must be greater than 0");
        RuleFor(x => x.Category)
            .NotEmpty()
            .Must(x => x.IsValidCompanyCategory())
            .WithMessage("Invalid category");
    }
}
