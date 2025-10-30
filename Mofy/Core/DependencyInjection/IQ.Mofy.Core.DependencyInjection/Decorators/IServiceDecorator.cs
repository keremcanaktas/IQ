using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.DependencyInjection.Services;

namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public interface IServiceDecorator : ISingletonInstance, IRequiredService
{
    object? Decorate(IServiceProvider serviceProvider, object? service);
}

public interface IServiceDecorator<T> : IServiceDecorator
{
    T Decorate(IServiceProvider serviceProvider, T service);
}