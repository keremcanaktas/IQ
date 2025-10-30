
// ReSharper disable ConvertIfStatementToReturnStatement

namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public abstract class ServiceDecorator<T> : IServiceDecorator<T>
{
    public object? Decorate(IServiceProvider serviceProvider, object? service)
    {
        if (service is not T tService) return service;

        return Decorate(serviceProvider, tService);
    }

    public abstract T Decorate(IServiceProvider serviceProvider, T service);
}