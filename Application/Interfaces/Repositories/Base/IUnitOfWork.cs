namespace SalesSystem.Application.Interfaces.Repositories.Base;

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
}