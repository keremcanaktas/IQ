using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.DependencyInjection.Annotations;
using IQ.Mofy.Data.Repositories;
using System.Linq.Expressions;

namespace ConsoleWebApplication;

public class CarRepository : ReadOnlyRepository<Car>, ICarRepository
{
    public override Task<Car?> GetAsync(Expression<Func<Car, bool>> predicate, CancellationToken cancellationToken = default)
    {
        var car = new Car
        {
            Id = 1
        };

        return Task.FromResult<Car?>(car);
    }

    public override Task<List<Car>> GetListAsync(CancellationToken cancellationToken = default)
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
        });
    }

    public override async Task<List<Car>> GetListAsync(Expression<Func<Car, bool>> predicate, CancellationToken cancellationToken = default) => (await GetListAsync(cancellationToken)).Where(predicate.Compile()).ToList();

    public IContext Context { get; set; } = null!;
}

[ServiceTypes(nameof(IApplication))]
public class MercedesCarRepository : CarRepository, IMercedesCarRepository
{
}