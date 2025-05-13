using IQ.Mofy.Core.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public class ServiceProvider(IServiceProvider inner) : IServiceProvider, IServiceScopeFactory, IKeyedServiceProvider
{
    public object? GetService(Type serviceType)
    {
        if (serviceType == typeof(IServiceScopeFactory))
            return this;

        var instance = inner.GetService(serviceType);

        if (instance != null)
            inner.GetService<IServiceProviderHandler>()?.Handle(this, serviceType, null, instance);

        return instance;
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (inner is not IKeyedServiceProvider keyedServiceProvider) return null;

        var instance = keyedServiceProvider.GetKeyedService(serviceType, serviceKey);

        if (instance != null)
            inner.GetService<IServiceProviderHandler>()?.Handle(this, serviceType, serviceKey, instance);

        return instance;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey) => GetKeyedService(serviceType, serviceKey) ?? throw new Exception("keyed");


    public IServiceScope CreateScope() => new ServiceScope(inner.GetRequiredService<IServiceScopeFactory>().CreateScope());
}


public class ServiceScope(IServiceScope inner) : IServiceScope
{
    public IServiceProvider ServiceProvider { get; protected set; } = new ServiceProvider(inner.ServiceProvider);

    public void Dispose() => inner.Dispose();
}