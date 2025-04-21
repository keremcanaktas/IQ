using Microsoft.EntityFrameworkCore;

namespace ConsoleWebApplication.Data;

public abstract class EfCoreIdentifierRepository<T, TIdentifier> : EfCoreRepository<T>, IReadonlyRepository<T, TIdentifier> where T : class, IEntity<TIdentifier>
{
    public virtual async Task<T?> GetAsync(TIdentifier id)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return await dbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetListAsync()
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return await dbContext.Set<T>().ToListAsync();
    }

    public virtual async Task<List<T>> GetListAsync(IEnumerable<TIdentifier> ids)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return await dbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}