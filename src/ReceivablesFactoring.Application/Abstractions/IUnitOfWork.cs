namespace ReceivablesFactoring.Application.Abstractions;

public interface IUnitOfWork
{
    Task<bool> Commit();
    Task Rollback();
}
