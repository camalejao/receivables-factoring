namespace ReceivablesFactoring.Domain.Exceptions;

public class ValidationFailureException : Exception
{
    public string[] Errors;

    public ValidationFailureException(string[] errors) : base()
    {
        Errors = errors;
    }

    public ValidationFailureException(string error) : base(error)
    {
        Errors = [error];
    }
}
