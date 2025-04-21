using System.Linq.Expressions;

namespace ConsoleWebApplication.Data;

public interface IReadonlyRepository<T> : IRepository where T : IEntity
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
    Task<IQueryable<T>> GetQueryableAsync();
}

public interface IReadonlyRepository<T, in TIdentifier> : IReadonlyRepository<T> where T : IEntity<TIdentifier>
{
    Task<T?> GetAsync(TIdentifier id);
    
    Task<List<T>> GetListAsync();
    
    Task<List<T>> GetListAsync(IEnumerable<TIdentifier> ids);
}