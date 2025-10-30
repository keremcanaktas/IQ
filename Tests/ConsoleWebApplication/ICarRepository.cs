using IQ.Mofy.Core.DependencyInjection.Accessors;
using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Data.Abstractions.Repositories;

namespace ConsoleWebApplication;

public interface IContext : ISingleton
{
    public string Name { get; set; }
}

public interface IContextAccessor : IAccessor
{
    public IContext Context { get; set; }
}

public class Context : IContext
{
    public string Name { get; set; } = "DataContext";
}

public interface ICarRepository : IReadOnlyRepository<Car>, IContextAccessor;

public interface IMercedesCarRepository : IReadOnlyRepository<Car>;