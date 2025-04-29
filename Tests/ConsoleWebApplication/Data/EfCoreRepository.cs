using IQ.Mofy.Core.Abstractions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace ConsoleWebApplication.Data;

public abstract class EfCoreRepository<T> : Repository<T>, IHasServiceProvider where T : class, IEntity
{
    public IServiceProvider ServiceProvider { get; set; } = null!;

    protected virtual IDbContextProvider DbContextProvider => ServiceProvider.GetRequiredService<IDbContextProvider>();

    public override async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) => (await GetQueryableAsync()).FirstOrDefault(predicate);

    public override async Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate) => await (await GetQueryableAsync()).Where(predicate).ToListAsync();

    public override async Task<IQueryable<T>> GetQueryableAsync() => (await DbContextProvider.ProvideAsync()).Set<T>();

    public override async Task AddAsync(T entity) => (await DbContextProvider.ProvideAsync()).Add(entity);

    public override async Task AddRangeAsync(IEnumerable<T> entities) => await (await DbContextProvider.ProvideAsync()).AddRangeAsync(entities);

    public override async Task UpdateAsync(T entity) => (await DbContextProvider.ProvideAsync()).Update(entity);

    public override async Task UpdateRangeAsync(IEnumerable<T> entities) => (await DbContextProvider.ProvideAsync()).UpdateRange(entities);

    public override async Task DeleteAsync(T entity) => (await DbContextProvider.ProvideAsync()).Remove(entity);

    public override async Task DeleteRangeAsync(IEnumerable<T> entities) => (await DbContextProvider.ProvideAsync()).RemoveRange(entities);
}