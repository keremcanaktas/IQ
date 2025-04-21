using Microsoft.EntityFrameworkCore;

namespace IQ.Test.Data;

public abstract class IdentifierRepository<T, TIdentifier> : Repository<T>, IReadonlyRepository<T, TIdentifier> where T : class, IEntity<TIdentifier>
{
    public virtual async Task<T?> GetAsync(TIdentifier id)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return await dbContext.Set<T>().FindAsync(id);
    }

    public virtual async Task<List<T>> GetListAsync(IEnumerable<TIdentifier> ids)
    {
        var dbContext = await DbContextProvider.ProvideAsync();
        return await dbContext.Set<T>().Where(x => ids.Contains(x.Id)).ToListAsync();
    }
}