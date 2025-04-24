using IQ.Mofy.Core.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

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
        if (!typeof(IHasServiceItem).IsAssignableFrom(serviceDescriptor.GetImplementationType() ?? serviceDescriptor.ServiceType))
        {
            self.Add(serviceDescriptor);
            return self;
        }

        self.Add(serviceDescriptor.DescribeWithKey());
        self.Add(new ServiceDescriptor(serviceType: serviceDescriptor.ServiceType, serviceKey: serviceDescriptor.ServiceKey, factory: GetInstance, lifetime: serviceDescriptor.Lifetime));

        return self;
    }

    private static object GetInstance(this IServiceProvider serviceProvider, object? key) => key is not ServiceDescriptorKey serviceItemKey ? default! : serviceProvider.GetInstance(serviceItemKey.ServiceType, serviceItemKey.Key);

    private static object GetInstance(this IServiceProvider serviceProvider, Type serviceType, object? serviceKey)
    {
        var instance = serviceProvider.GetRequiredKeyedService(serviceType, new ServiceDescriptorKey(serviceType, serviceKey));

        AddServiceItems();

        return instance;

        void AddServiceItems()
        {
            switch (instance)
            {
                case null:
                    return;
                case IHasServiceCollection hasServiceCollection:
                    hasServiceCollection.ServiceCollection = serviceProvider.GetRequiredService<IServiceCollection>();
                    break;
                case IHasServiceProvider hasServiceProvider:
                    hasServiceProvider.ServiceProvider = serviceProvider;
                    break;
            }
        }
    }
}