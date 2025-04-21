using IQ.Mofy.Core.Data.Annotations.DependencyInjection;
using IQ.Test.Data.Integer;
using System.Linq.Expressions;

namespace IQ.Test;

public interface ICarRepository : IRepository<Car>;

[ServiceTypes(Key = CarType.Mercedes)]
public class MercedesCarRepository : Repository<Car>, ICarRepository
{
    public override Task<Car?> GetAsync(Expression<Func<Car, bool>> predicate) => Task.FromResult<Car?>(new Car
    {
        Id = 1,
        Name = "Mercedes"
    });
}

public enum CarType
{
    Mercedes,
    BMW,
    Bugatti,
    Porche
}