using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IServiceProviderHandler : ISingletonInstance
{
    void Handle(IServiceProvider serviceProvider, Type type, object? serviceKey, object? instance);
}