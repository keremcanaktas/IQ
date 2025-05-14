using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection.Accessors;

public interface IServiceCollectionAccessor : IAccessor
{
    IServiceCollection ServiceCollection { get; set; }
}