using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Data.Abstractions.Entities;
using System.Linq.Expressions;

namespace IQ.Mofy.Data.Abstractions.Repositories;

public interface IRepository : IScoped;

public interface ICreateOnlyRepository<T> : IRepository
    where T : IEntity
{
    Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<T> : IRepository 
    where T : IEntity
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync(CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}

public interface IReadOnlyRepository<T, in TKey> : IReadOnlyRepository<T>
    where T : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<T?> GetAsync(TKey key, CancellationToken cancellationToken = default);

    Task<List<T>> GetListAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}

public interface IQueryableRepository<T> : IReadOnlyRepository<T>, IQueryable<T>
    where T : IEntity;

public interface IUpdateOnlyRepository<T> : IRepository
    where T : IEntity
{
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IDeleteOnlyRepository<T> : IRepository
    where T : IEntity
{
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IDeleteOnlyRepository<T, TKey> : IDeleteOnlyRepository<T>
    where T : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}



public interface IRepository<T> : ICreateOnlyRepository<T>, IReadOnlyRepository<T>, IUpdateOnlyRepository<T>, IDeleteOnlyRepository<T>
    where T : IEntity;

public interface IRepository<T, TKey> : IRepository<T>, IReadOnlyRepository<T, TKey>, IDeleteOnlyRepository<T, TKey>
    where T : IEntity<TKey>
    where TKey : IEquatable<TKey>;



public interface IImmediateCreateOnlyRepository<T> : ICreateOnlyRepository<T> 
    where T : IEntity
{
    new Task CreateAsync(T entity, CancellationToken cancellationToken = default);
    new Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IImmediateUpdateOnlyRepository<T> : IUpdateOnlyRepository<T>
    where T : IEntity
{
    new Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    new Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IImmediateDeleteOnlyRepository<T> : IDeleteOnlyRepository<T>
    where T : IEntity
{
    new Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    new Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
}

public interface IImmediateDeleteOnlyRepository<T, TKey> : IImmediateDeleteOnlyRepository<T>, IDeleteOnlyRepository<T, TKey>
    where T : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    new Task DeleteAsync(TKey key, CancellationToken cancellationToken = default);
    new Task DeleteRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);
}

public interface IImmediateRepository<T> : IImmediateCreateOnlyRepository<T>, IImmediateUpdateOnlyRepository<T>, IImmediateDeleteOnlyRepository<T>
    where T : IEntity;



public interface ISupportSaveChanges
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}