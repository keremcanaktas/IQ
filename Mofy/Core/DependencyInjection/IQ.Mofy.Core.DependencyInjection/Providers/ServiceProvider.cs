
using IQ.Mofy.Core.DependencyInjection.Accessors;
using IQ.Mofy.Core.DependencyInjection.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection.Providers;

public class ServiceProvider(IServiceProvider inner) : IServiceProvider, ISupportRequiredService, IServiceScopeFactory, IServiceProviderIsService
{
    public object? GetService(Type serviceType)
    {
        if (typeof(IServiceScopeFactory).IsAssignableFrom(serviceType)) return this;

        var instance = inner.GetService(serviceType);

        ApplyAccessors(instance);
        instance = ApplyDecorators(instance);

        return instance;
    }

    public object GetRequiredService(Type serviceType) => GetService(serviceType) ?? throw new InvalidOperationException($"No service for type '{serviceType.Name}' has been registered.");

    public IServiceScope CreateScope() => new ServiceScope(inner.CreateScope());

    public bool IsService(Type serviceType) => inner.GetRequiredService<IServiceProviderIsService>().IsService(serviceType);


    private void ApplyAccessors(object? instance)
    {
        if (instance is IServiceProviderAccessor serviceProviderAccessor)
            serviceProviderAccessor.ServiceProvider = this;

        if (instance is IServiceCollectionAccessor serviceCollectionAccessor)
            serviceCollectionAccessor.ServiceCollection = inner.GetRequiredService<IServiceCollection>();
    }


    private object? ApplyDecorators(object? instance)
    {
        if (instance is null) return instance;

        foreach (var decorator in inner.GetServices<IServiceDecorator>())
            instance = decorator.Decorate(this, instance);

        return instance;
    }
}

public class ServiceScope(IServiceScope innerScope) : IServiceScope
{
    public IServiceProvider ServiceProvider { get; } = new ServiceProvider(innerScope.ServiceProvider);

    public void Dispose() => innerScope.Dispose();
}