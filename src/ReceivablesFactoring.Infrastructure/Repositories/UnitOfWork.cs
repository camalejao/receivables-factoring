using ReceivablesFactoring.Application.Abstractions;
using ReceivablesFactoring.Infrastructure.Database;

namespace ReceivablesFactoring.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    
    public UnitOfWork(ApplicationDbContext applicationDbContext)
    {
        _dbContext = applicationDbContext;
    }

    public async Task<bool> Commit()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public Task Rollback()
    {
        return Task.CompletedTask;
    }
}
