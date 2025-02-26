namespace ReceivablesFactoring.Application.Abstractions;

public interface IDateProvider
{
    DateOnly Today { get; }
}
