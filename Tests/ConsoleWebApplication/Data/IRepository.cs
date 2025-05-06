using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace ConsoleWebApplication.Data;

public interface IRepository : IScoped, IAsyncDisposable, IDisposable;

public interface IRepository<T> : IReadonlyRepository<T>, IRequiredService where T : IEntity
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
        
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
        
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
}

public interface IRepository<T, TIdentifier> : IRepository<T> where T : IEntity
{
    Task DeleteAsync(TIdentifier id);
    Task DeleteRangeAsync(IEnumerable<TIdentifier> ids);
}
