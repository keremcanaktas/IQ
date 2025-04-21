using System.Linq.Expressions;
using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Fundamentals.Disposable;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Test.Data;

public abstract class Repository<T> : AsyncDisposable, IRepository<T>, IHasServiceProvider where T : class, IEntity
{
    public IServiceProvider ServiceProvider { get; set; } = null!;

    protected virtual IDbContextProvider DbContextProvider => ServiceProvider.GetRequiredService<IDbContextProvider>();
        
        
    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
            
        return dbContext.Set<T>().FirstOrDefault(predicate);
    }

    public virtual async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
            
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public virtual async Task<IQueryable<T>> GetQueryableAsync()
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return dbContext.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        dbContext.Add(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        var dbContext = await DbContextProvider.ProvideAsync(); 
        await dbContext.AddRangeAsync(entities);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var dbContext = await DbContextProvider.ProvideAsync(); 
        dbContext.Update(entity);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        var dbContext = await DbContextProvider.ProvideAsync(); 
        dbContext.UpdateRange(entities);
    }

    public virtual async Task DeleteAsync(T entity)
    {
        var dbContext = await DbContextProvider.ProvideAsync(); 
        dbContext.Remove(entity);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        var dbContext = await DbContextProvider.ProvideAsync(); 
        dbContext.RemoveRange(entities);
    }
}