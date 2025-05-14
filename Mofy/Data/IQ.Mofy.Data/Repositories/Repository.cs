using IQ.Mofy.Core.DependencyInjection.Accessors;
using IQ.Mofy.Core.Fundamentals.Disposable;
using IQ.Mofy.Data.Abstractions.Entities;
using IQ.Mofy.Data.Abstractions.Repositories;
using System.Collections;
using System.Linq.Expressions;

namespace IQ.Mofy.Data.Repositories;

public abstract class Repository : Disposable, IServiceProviderAccessor
{
    #region IHasServiceProvider

    public IServiceProvider ServiceProvider { get; set; } = default!;

    #endregion
}

public abstract class ReadOnlyRepository<T> : Repository, IReadOnlyRepository<T>
    where T : IEntity
{
    #region IReadOnlyRepository<T>

    public abstract Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    public abstract Task<List<T>> GetListAsync(CancellationToken cancellationToken = default);

    public abstract Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    #endregion
}

public abstract class QueryableRepository<T>(IQueryable<T> queryable) : ReadOnlyRepository<T>, IQueryableRepository<T>
    where T : IEntity
{
    protected virtual IQueryable<T> Queryable { get; } = queryable;

    #region IQueryable<T>

    public Type ElementType => Queryable.ElementType;

    public Expression Expression => Queryable.Expression;

    public IQueryProvider Provider => Queryable.Provider;

    public IEnumerator<T> GetEnumerator() => Queryable.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Queryable.GetEnumerator();

    #endregion
}

public abstract class CrudRepository<T> : ReadOnlyRepository<T>, IRepository<T>
    where T : IEntity
{
    #region ICreateOnlyRepository<T>

    public abstract Task CreateAsync(T entity, CancellationToken cancellationToken = default);

    public abstract Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #endregion

    #region IUpdateOnlyRepository<T>

    public abstract Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

    public abstract Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #endregion

    #region IDeleteOnlyRepository<T>

    public abstract Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    public abstract Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    #endregion
}

public abstract class ImmediateRepository<T> : CrudRepository<T>, IImmediateCreateOnlyRepository<T>, IImmediateUpdateOnlyRepository<T>, IImmediateDeleteOnlyRepository<T>, ISupportSaveChanges
    where T : IEntity
{
    #region IImmediateCreateOnlyRepository<T>

    async Task IImmediateCreateOnlyRepository<T>.CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await CreateAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    async Task IImmediateCreateOnlyRepository<T>.CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await CreateRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region IImmediateUpdateOnlyRepository<T>

    async Task IImmediateUpdateOnlyRepository<T>.UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        await UpdateAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    async Task IImmediateUpdateOnlyRepository<T>.UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await UpdateRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region IImmediateDeleteOnlyRepository<T>

    async Task IImmediateDeleteOnlyRepository<T>.DeleteAsync(T entity, CancellationToken cancellationToken)
    {
        await DeleteAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    async Task IImmediateDeleteOnlyRepository<T>.DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
    {
        await DeleteRangeAsync(entities, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    #endregion

    public abstract Task SaveChangesAsync(CancellationToken cancellationToken = default);
}