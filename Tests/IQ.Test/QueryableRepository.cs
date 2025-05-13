using IQ.Mofy.Data.Abstractions.Entities;
using IQ.Mofy.Data.Repositories;
using System.Linq.Expressions;

namespace IQ.Test;

//public class InMemoryQueryableRepository<T, TKey> : QueryableRepository<T, TKey>
//    where T : IEntity<TKey>
//    where TKey : IEquatable<TKey>
//{
//    public InMemoryQueryableRepository() : base(new List<T>().AsQueryable()) { }

//    protected virtual List<T> Cache { get; set; } = new List<T>();

//    protected override IQueryable<T> Queryable => Cache.AsQueryable();

//    public override Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Task.FromResult(this.FirstOrDefault(predicate.Compile()));

//    public override Task<List<T>> GetListAsync(CancellationToken cancellationToken = default) => Task.FromResult(this.ToList());

//    public override Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Task.FromResult(this.Where(predicate).ToList());


//    public override Task CreateAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        Cache.Add(entity);
//        return Task.CompletedTask;
//    }

//    public override Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        Cache.AddRange(entities);
//        return Task.CompletedTask;
//    }

//    public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        var index = Cache.IndexOf(entity);

//        if (index >= 0)
//            Cache[index] = entity;
//        else 
//            await CreateAsync(entity, cancellationToken);
//    }

//    public override async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        var tasks = entities.Select(entity => UpdateAsync(entity, cancellationToken));

//        await Task.WhenAll(tasks);
//    }

//    public override Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        Cache.Remove(entity);
//        return Task.CompletedTask;
//    }

//    public override async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        var tasks = entities.Select(entity => DeleteAsync(entity, cancellationToken));

//        await Task.WhenAll(tasks);
//    }


//    public override Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
//}

//public class InMemoryQueryableRepository<T> : InMemoryQueryableRepository<T, Guid>
//    where T : IEntity<Guid>
//{
//}


//public class EFImmadiateRepository<T, TKey> : ImmediateRepository<T, TKey>
//    where T : IEntity<TKey>
//    where TKey : IEquatable<TKey>
//{
//    protected virtual List<T> Cache { get; set; } = new List<T>();

//    public override Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Task.FromResult(Cache.FirstOrDefault(predicate.Compile()));

//    public override Task<List<T>> GetListAsync(CancellationToken cancellationToken = default) => Task.FromResult(Cache.ToList());

//    public override Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) => Task.FromResult(Cache.Where(predicate.Compile()).ToList());


//    public override Task CreateAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        Cache.Add(entity);
//        return Task.CompletedTask;
//    }

//    public override Task CreateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        Cache.AddRange(entities);
//        return Task.CompletedTask;
//    }

//    public override async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        var index = Cache.IndexOf(entity);

//        if (index >= 0)
//            Cache[index] = entity;
//        else
//            await CreateAsync(entity, cancellationToken);
//    }

//    public override async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        var tasks = entities.Select(entity => UpdateAsync(entity, cancellationToken));

//        await Task.WhenAll(tasks);
//    }

//    public override Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
//    {
//        Cache.Remove(entity);
//        return Task.CompletedTask;
//    }

//    public override async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
//    {
//        var tasks = entities.Select(entity => DeleteAsync(entity, cancellationToken));

//        await Task.WhenAll(tasks);
//    }

//    public override Task SaveChangesAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
//}



//public class EFImmadiateRepository<T> : EFImmadiateRepository<T, int>
//    where T : IEntity<int>
//{

//}