using System.Linq.Expressions;

namespace IQ.Test.Data;

public interface IReadonlyRepository<T> : IRepository where T : class, IEntity
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
        
    Task<IQueryable<T>> GetQueryableAsync();
}

public interface IReadonlyRepository<T, in TIdentifier> : IReadonlyRepository<T> where T : class, IEntity<TIdentifier>
{
    Task<T?> GetAsync(TIdentifier id);
    Task<List<T>> GetListAsync(IEnumerable<TIdentifier> ids);
}