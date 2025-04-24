using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasReadonlyServiceCollection : IHasServiceCollectionItem
{
    public IServiceCollection ServiceCollection { get; }
}