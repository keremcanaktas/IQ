using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;
using IQ.Mofy.Data.Abstractions.Entities;
using System.Linq.Expressions;

namespace IQ.Mofy.Data.Abstractions.Repositories;

public interface IRepository : IScoped;

public interface ISupportSpecification<T> where T : class
{
    Task<T?> GetAsync(ISpecification<T> specification);

    Task<List<T>> GetListAsync(ISpecification<T> specification);
}

public interface IReadonlyRepository<T> : IRepository 
    where T : IEntity
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> GetListAsync();

    Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
}

public interface IReadonlyRepository<T, in TId> : IReadonlyRepository<T>
    where T : IEntity<TId>
    where TId : IEquatable<TId>
{
    Task<T?> GetAsync(TId id);

    Task<List<T>> GetListAsync(IEnumerable<TId> ids);
}

public interface ICreateRepository<T> : IRepository 
    where T : IEntity
{
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
}

public interface IUpdateRepository<T> : IRepository
    where T : IEntity
{
    Task UpdateAsync(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities);
}

public interface IDeleteRepository<T> : IRepository
    where T : IEntity
{
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(IEnumerable<T> entities);
}

public interface IDeleteRepository<T, TId> : IDeleteRepository<T>
    where T : IEntity<TId>
    where TId : IEquatable<TId>
{
    Task DeleteAsync(TId id);
    Task DeleteRangeAsync(IEnumerable<TId> ids);
}




public interface IRepository<T> : IReadonlyRepository<T>, ICreateRepository<T>, IUpdateRepository<T>, IDeleteRepository<T> 
    where T : IEntity;

public interface IRepository<T, TId> : IRepository<T>, IDeleteRepository<T, TId> 
    where T : IEntity<TId> 
    where TId : IEquatable<TId>;


public interface IQueryableRepository<T> : IRepository<T>, IQueryable<T>
    where T : IEntity;

public interface IQueryableRepository<T, TId> : IQueryableRepository<T>, IRepository<T, TId>, IQueryable<T>
    where T : IEntity<TId>
    where TId : IEquatable<TId>;



public interface ISpecification<T>
{
    bool IsSatisfiedBy(T obj);

    Expression<Func<T, bool>> ToExpression();
}








public static class Extensions
{
    public static IQueryable<T> And<T>(this IQueryable<T> self, ISpecification<T> specification) => self.Where(specification.ToExpression());

    public static ISpecification<T> And<T>(this ISpecification<T> self, ISpecification<T> specification) => new AndSpecification<T>(self, specification);
    public static ISpecification<T> Or<T>(this ISpecification<T> self, ISpecification<T> specification) => new OrSpecification<T>(self, specification);
    public static ISpecification<T> Equal<T>(this ISpecification<T> self, ISpecification<T> specification) => new EqualSpecification<T>(self, specification);
    public static ISpecification<T> NotEqual<T>(this ISpecification<T> self, ISpecification<T> specification) => new EqualSpecification<T>(self, specification);
}


public abstract class Specification<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> ToExpression();
    public bool IsSatisfiedBy(T entity)
    {
        Func<T, bool> predicate = ToExpression().Compile();
        return predicate(entity);
    }
}


public class AndSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>((BinaryExpression)new ParameterReplacer(parameterExpression).Visit(Expression.And(left.ToExpression().Body, right.ToExpression().Body)), parameterExpression);
    }
}

public class OrSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>((BinaryExpression)new ParameterReplacer(parameterExpression).Visit(Expression.Or(left.ToExpression().Body, right.ToExpression().Body)), parameterExpression);
    }
}


public class EqualSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>((BinaryExpression)new ParameterReplacer(parameterExpression).Visit(Expression.Equal(left.ToExpression().Body, right.ToExpression().Body)), parameterExpression);
    }
}

public class NotEqualSpecification<T>(ISpecification<T> left, ISpecification<T> right) : Specification<T>
{
    public override Expression<Func<T, bool>> ToExpression()
    {
        var parameterExpression = Expression.Parameter(typeof(T));

        return Expression.Lambda<Func<T, bool>>((BinaryExpression)new ParameterReplacer(parameterExpression).Visit(Expression.NotEqual(left.ToExpression().Body, right.ToExpression().Body)), parameterExpression);
    }
}


class ParameterReplacer(ParameterExpression parameter) : ExpressionVisitor
{   
    protected override Expression VisitParameter(ParameterExpression node) => base.VisitParameter(parameter);
}

public class Product : IEntity<int>
{
    public int Id { get; set; }

    public string? Name { get; set; }
    
    public int Count { get; set; }
}

public class HasStockSpecification : Specification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() => p => p.Count > 0;
}

public class NameFilterSpecification(string name) : Specification<Product>
{
    public override Expression<Func<Product, bool>> ToExpression() => p => p.Name == name;
}


public class Service
{
    public Service()
    {
        var specification = new HasStockSpecification()
            .And(new NameFilterSpecification("Pencil"))
            .Equal(new HasStockSpecification());


        new List<Product>().AsQueryable().And(specification);
    }
}