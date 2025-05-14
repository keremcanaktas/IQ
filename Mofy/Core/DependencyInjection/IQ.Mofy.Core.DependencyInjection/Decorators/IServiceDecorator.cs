namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public interface IServiceDecorator
{
    void Decorate(IServiceProvider serviceProvider, object? instance);
}

public interface IServiceDecorator<in T> : IServiceDecorator
{
    void Decorate(IServiceProvider serviceProvider, T instance);
}

public abstract class ServiceDecorator<T> : IServiceDecorator<T>
{
    public void Decorate(IServiceProvider serviceProvider, object? instance)
    {
        if (instance is not T @object) return;

        Decorate(serviceProvider, @object);
    }

    public abstract void Decorate(IServiceProvider serviceProvider, T instance);
}