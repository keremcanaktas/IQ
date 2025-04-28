using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;
using IQ.Mofy.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace IQ.Mofy.Core.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    #region AddSingleton

    #region Keyed

    public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection self, object? serviceKey, TService instance) where TService : class => self.Append(ServiceDescriptor.KeyedSingleton(serviceKey, instance));

    public static IServiceCollection AddKeyedSingleton<TService>(this IServiceCollection self, object? serviceKey) where TService : class => self.Append(ServiceDescriptor.KeyedSingleton<TService, TService>(serviceKey));

    public static IServiceCollection AddKeyedSingleton<TService, TImplementation>(this IServiceCollection self, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.KeyedSingleton<TService, TImplementation>(serviceKey));
    }

    #endregion

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection self, TService instance) where TService : class => self.Append(ServiceDescriptor.Singleton(instance));

    public static IServiceCollection AddSingleton<TService>(this IServiceCollection self) where TService : class => self.Append(ServiceDescriptor.Singleton<TService, TService>());

    public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection self)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.Singleton<TService, TImplementation>());
    }

    #endregion

    #region AddTransient

    #region Keyed

    public static IServiceCollection AddKeyedTransient<TService>(this IServiceCollection self, object serviceKey) where TService : class => self.Append(ServiceDescriptor.KeyedTransient<TService, TService>(serviceKey));

    public static IServiceCollection AddKeyedTransient<TService, TImplementation>(this IServiceCollection self, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.KeyedTransient<TService, TImplementation>(serviceKey));
    }

    #endregion

    public static IServiceCollection AddTransient<TService>(this IServiceCollection self) where TService : class => self.Append(ServiceDescriptor.Transient<TService, TService>());

    public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection self)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.Transient<TService, TImplementation>());
    }

    #endregion

    #region AddScoped

    #region Keyed

    public static IServiceCollection AddKeyedScoped<TService>(this IServiceCollection self, object serviceKey) where TService : class => self.Append(ServiceDescriptor.KeyedScoped<TService, TService>(serviceKey));

    public static IServiceCollection AddKeyedScoped<TService, TImplementation>(this IServiceCollection self, object serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.KeyedScoped<TService, TImplementation>(serviceKey));
    }

    #endregion

    public static IServiceCollection AddScoped<TService>(this IServiceCollection self) where TService : class => self.Append(ServiceDescriptor.Scoped<TService, TService>());

    public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection self)
        where TService : class
        where TImplementation : class, TService
    {
        return self.Append(ServiceDescriptor.Scoped<TService, TImplementation>());
    }

    #endregion


    private static IServiceCollection Append(this IServiceCollection self, ServiceDescriptor serviceDescriptor)
    {
        var implementationType = serviceDescriptor.GetImplementationType() ?? serviceDescriptor.ServiceType;

        if (!typeof(IHasServiceCollectionItem).IsAssignableFrom(implementationType))
        {
            self.Add(serviceDescriptor);
            return self;
        }

        if (serviceDescriptor.GetImplementationInstance() is IHasServiceCollection hasServiceCollection) hasServiceCollection.ServiceCollection = self;

        if (typeof(ISingletonInstance).IsAssignableFrom(implementationType))
            return Add<ISingletonInstance>();

        return Add<object>();
        
        IServiceCollection Add<TImplementation>() where TImplementation : class
        {
            if(!serviceDescriptor.IsKeyedService)
                self.Add(new ServiceDescriptor(serviceType: serviceDescriptor.ServiceType, factory: new Func<IServiceProvider, TImplementation>(sp => (TImplementation)sp.GetInstance(serviceDescriptor)), lifetime: serviceDescriptor.Lifetime));
            else
                self.Add(new ServiceDescriptor(serviceType: serviceDescriptor.ServiceType, serviceKey: serviceDescriptor.ServiceKey, factory: new Func<IServiceProvider, object?, TImplementation>((sp, k) => (TImplementation)sp.GetInstance(serviceDescriptor)), lifetime: serviceDescriptor.Lifetime));
            return self;
        }
    }

    private static object GetInstance(this IServiceProvider serviceProvider, ServiceDescriptor serviceDescriptor)
    {
        var instance = serviceDescriptor.ImplementationInstance
                       ?? serviceDescriptor.ImplementationFactory?.Invoke(serviceProvider)
                       ?? (serviceDescriptor.ImplementationType is not null ? ActivatorUtilities.CreateInstance(serviceProvider, serviceDescriptor.ImplementationType) : null);

        if (serviceDescriptor.IsKeyedService)
            instance ??= serviceDescriptor.KeyedImplementationInstance
                         ?? serviceDescriptor.KeyedImplementationFactory?.Invoke(serviceProvider, serviceDescriptor.ServiceKey)
                         ?? (serviceDescriptor.KeyedImplementationType is not null ? ActivatorUtilities.CreateInstance(serviceProvider, serviceDescriptor.KeyedImplementationType) : null);

        AddServiceItems();

        return instance!;

        void AddServiceItems()
        {
            if (instance is IHasServiceCollection hasServiceCollection)
                hasServiceCollection.ServiceCollection = serviceProvider.GetService<IServiceCollection>()!;

            if (serviceProvider is IEmptyServiceProvider) return;

            if (instance is IHasServiceProvider hasServiceProvider)
                hasServiceProvider.ServiceProvider = serviceProvider;

            foreach (var serviceItemDecorator in serviceProvider.GetServices<IServiceCollectionItemDecorator>())
                serviceItemDecorator.Decorate(serviceProvider, instance);
        }
    }
}