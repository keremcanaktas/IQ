using System.Linq.Expressions;
using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Fundamentals.Disposable;

namespace ConsoleWebApplication.Data;

public abstract class Repository<T> : AsyncDisposable, IRepository<T> where T : class, IEntity
{
    public virtual Task<T?> GetAsync(Expression<Func<T, bool>> predicate) => Task.FromResult(default(T));
    public virtual Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate) => Task.FromResult(new List<T>());
    public virtual Task<IQueryable<T>> GetQueryableAsync() => Task.FromResult(Enumerable.Empty<T>().AsQueryable());

    public virtual Task AddAsync(T entity) => Task.CompletedTask;

    public virtual Task AddRangeAsync(IEnumerable<T> entities) => Task.CompletedTask;

    public virtual Task UpdateAsync(T entity) => Task.CompletedTask;

    public virtual Task UpdateRangeAsync(IEnumerable<T> entities) => Task.CompletedTask;

    public virtual Task DeleteAsync(T entity) => Task.CompletedTask;

    public virtual Task DeleteRangeAsync(IEnumerable<T> entities) => Task.CompletedTask;
}