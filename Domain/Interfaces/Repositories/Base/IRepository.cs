using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories.Base;

public interface IRepository<T>
{
    IQueryable<T> Query();
    Task<List<T>> GetAllAsync();

    IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    Task<T?> FindAsync(long id);

    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    T Update(T entity, bool save = true);
    Task<T> InsertAsync(T entity, bool save = true);
    Task<List<T>> InsertAllAsync(List<T> entity, bool save = true);
    Task<T> PatchAsync(T entity, params Expression<Func<T, object?>>[] propertyExpressions);

    Task<T> UpdateAsync(T entity, bool save = true);
    Task DeleteAsync(T entity, bool save = true);
    Task DeleteAllAsync(List<T> entities, bool save = true);
    Task SaveChangesAsync();
    Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool save = true);
}