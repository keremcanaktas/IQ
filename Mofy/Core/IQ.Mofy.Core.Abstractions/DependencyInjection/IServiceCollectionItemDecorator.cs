using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IServiceCollectionItemDecorator : ISingleton, IServiceTypeRequired
{
    void Decorate(IServiceProvider serviceProvider, object? instance);
}

public interface IServiceCollectionItemDecorator<in T> : IServiceCollectionItemDecorator where T : IHasServiceCollectionItem
{
    void Decorate(IServiceProvider serviceProvider, T instance);
}

public abstract class ServiceCollectionItemDecorator<T> : IServiceCollectionItemDecorator<T> where T : IHasServiceCollectionItem
{
    public void Decorate(IServiceProvider serviceProvider, object? instance)
    {
        if (instance is not T hasServiceItem) return;

        Decorate(serviceProvider, hasServiceItem);
    }

    public abstract void Decorate(IServiceProvider serviceProvider, T instance);
}