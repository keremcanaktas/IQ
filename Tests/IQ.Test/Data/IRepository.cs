using IQ.Mofy.Core.Abstractions.DependencyInjection.Services;

namespace IQ.Test.Data;

public interface IRepository : IScoped, IAsyncDisposable, IDisposable;

public interface IRepository<T> : IReadonlyRepository<T> where T : class, IEntity
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
        
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
        
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
}
