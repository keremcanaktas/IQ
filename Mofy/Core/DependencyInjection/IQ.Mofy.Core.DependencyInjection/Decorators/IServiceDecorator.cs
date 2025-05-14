using IQ.Mofy.Core.DependencyInjection.Descriptors;

namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public interface IServiceDecorator : ISingletonInstance
{
    object? Decorate(IServiceProvider serviceProvider, object? instance);
}

public interface IServiceDecorator<T> : IServiceDecorator
{
    T Decorate(IServiceProvider serviceProvider, T instance);
}

public abstract class ServiceDecorator<T> : IServiceDecorator<T>
{
    public object? Decorate(IServiceProvider serviceProvider, object? instance)
    {
        if (instance is not T @object) return instance;

        return Decorate(serviceProvider, @object);
    }

    public abstract T Decorate(IServiceProvider serviceProvider, T instance);
}