using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasReadonlyServiceCollection
{
    public IServiceCollection ServiceCollection { get; }
}