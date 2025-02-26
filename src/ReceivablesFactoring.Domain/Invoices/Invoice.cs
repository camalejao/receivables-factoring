using ReceivablesFactoring.Domain.Companies;

namespace ReceivablesFactoring.Domain.Invoices;

public class Invoice
{
    public Guid Id { get; set; }

    public Guid CompanyId { get; set; }

    public int Number { get; set; }

    public DateOnly DueDate { get; set; }
    
    public decimal Value { get; set; }
    
    public bool InCart { get; set; }

    public Company Company { get; set; }
}
