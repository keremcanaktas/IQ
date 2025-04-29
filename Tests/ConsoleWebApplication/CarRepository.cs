using ConsoleWebApplication.Data.Integer;
using System.Linq.Expressions;

namespace ConsoleWebApplication;

public class CarRepository : EfCoreRepository<Car>, ICarRepository
{
    public override Task<Car?> GetAsync(int id)
    {
        var car = new Car
        {
            Id = id
        };

        return Task.FromResult<Car?>(car);
    }

    public override Task<List<Car>> GetListAsync(IEnumerable<int> ids)
    {
        return new(() => ids.Select(id => new Car { Id = id }).ToList());
    }

    public override Task<List<Car>> GetListAsync(Expression<Func<Car, bool>> predicate)
    {
        return Task.FromResult(new List<Car>
        {
            new() { Id = 1, Name = "Audi" },
            new() { Id = 2, Name = "Mercedes" },
            new() { Id = 3, Name = "BMW" },
            new() { Id = 4, Name = "Toyota" },
            new() { Id = 5, Name = "Honda" },
            new() { Id = 6, Name = "Ford" },
            new() { Id = 7, Name = "Chevrolet" },
            new() { Id = 8, Name = "Nissan" },
            new() { Id = 9, Name = "Volkswagen" },
            new() { Id = 10, Name = "Hyundai" }
        }.Where(predicate.Compile()).ToList());
    }

    protected override void ReleaseManagedResources()
    {
        base.ReleaseManagedResources();
    }
}

public class MercedesRepository : CarRepository, IMercedesRepository
{
    protected override void ReleaseManagedResources()
    {
        base.ReleaseManagedResources();
    }
}