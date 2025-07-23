using Infrastructure.Persistence;

namespace Infrastructure.Repositories.Base;

public interface IUnitOfWork
{
    void Save();

    bool BeginTransaction();

    Task BeginTransactionAsync();

    void RollBack();
    Task RollBackAsync();

    void Commit();

    Task SaveAsync();

    Task CommitAsync();

    SalesDbContext GetContext();
}
public class UnitOfWork : IUnitOfWork
{
    private readonly SalesDbContext _ctx;

    public UnitOfWork(SalesDbContext dbContext)
    {
        _ctx = dbContext;
    }

    public void Save()
    {
        _ctx.SaveChanges();
    }

    public Task SaveAsync()
    {
        return _ctx.SaveChangesAsync();
    }

    public bool BeginTransaction()
    {
        if (_ctx.Database.CurrentTransaction is not null)
            return false;
        _ctx.Database.BeginTransaction();
        return true;
    }

    public Task BeginTransactionAsync()
    {
        if (_ctx.Database.CurrentTransaction is not null)
            return Task.CompletedTask;
        return _ctx.Database.BeginTransactionAsync();
    }

    public void Commit()
    {
        if (_ctx.Database.CurrentTransaction is null)
            return;
        _ctx.Database.CommitTransaction();
    }

    public Task CommitAsync()
    {
        if (_ctx.Database.CurrentTransaction is null)
            return Task.CompletedTask;

        return _ctx.Database.CommitTransactionAsync();
    }

    public void RollBack()
    {
        if (_ctx.Database.CurrentTransaction is null)
            return;

        _ctx.Database.RollbackTransaction();
    }

    public Task RollBackAsync()
    {
        if (_ctx.Database.CurrentTransaction is null)
            return Task.CompletedTask;
        return _ctx.Database.RollbackTransactionAsync();
    }

    public SalesDbContext GetContext()
    {
        return _ctx;
    }
}