using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SalesSystem.Application.Interfaces.Repositories.Base;
using SalesSystem.Infrastructure.Persistence;
using SalesSystem.Shared.Extensions;

namespace SalesSystem.Infrastructure.Repositories.Base;

public abstract class Repository<T>(SalesDbContext context) : IRepository<T> where T : class
{
    protected readonly SalesDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public virtual IQueryable<T> Query() => _dbSet.AsNoTracking().AsQueryable();

    public virtual IQueryable<T> Query(Dictionary<string, string>? filters, string? order)
    {
        var query = _dbSet.AsNoTracking().AsQueryable();
        if (filters != null) query = query.ApplyFilters(filters);
        if (order != null) query = query.ApplyOrdering(order);
        return query;
    }

    public virtual async Task<List<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate) => _dbSet.Where(predicate);

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate) =>
        await Query().AnyAsync(predicate);

    public virtual async Task<T?> FindAsync(long id) => await _dbSet.FindAsync(id);

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<T> InsertAsync(T entity, bool save = true)
    {
        await _dbSet.AddAsync(entity);

        if (save)
            await SaveChangesAsync();

        return entity;
    }

    public async Task<List<T>> InsertAllAsync(List<T> entity, bool save = true)
    {
        await _dbSet.AddRangeAsync(entity);
        if (save)
            await SaveChangesAsync();
        return entity;
    }

    public T Update(T entity, bool save = true)
    {
        _dbSet.Update(entity);

        if (save)
            SaveChanges();

        return entity;
    }

    public async Task<T> PatchAsync(
        T entity,
        params Expression<Func<T, object?>>[] propertyExpressions
    )
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Unchanged;

        ModifyProps(entity, propertyExpressions);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity, bool save = true)
    {
        _dbSet.Update(entity);

        if (save)
            await SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities, bool save = true)
    {
        _dbSet.UpdateRange(entities);

        if (save)
            await SaveChangesAsync();

        return entities;
    }

    public async Task DeleteAsync(T entity, bool save = true)
    {
        _dbSet.Remove(entity);

        if (save)
            await SaveChangesAsync();
    }

    public async Task DeleteAllAsync(List<T> entities, bool save = true)
    {
        if (entities == null || entities.Count == 0)
            throw new ArgumentException("The list of entities to delete cannot be null or empty.", nameof(entities));

        _dbSet.RemoveRange(entities);

        if (save)
            await SaveChangesAsync();
    }

    public void SaveChanges() => _context.SaveChanges();

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    /// <summary>
    /// Define todas as propriedades nos parâmetros de expressão como modificadas.
    /// </summary>
    private void ModifyProps(T entity, params Expression<Func<T, object?>>[] propertyExpressions)
    {
        foreach (var propertyExpression in propertyExpressions)
        {
            _dbSet.Entry(entity).Property(propertyExpression).IsModified = true;
        }
    }
}