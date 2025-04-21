using IQ.Mofy.Core.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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


    public static IServiceCollection Append(this IServiceCollection self, ServiceDescriptor serviceDescriptor)
    {
        void Configure()
        {
            if (!typeof(IHasServiceItem).IsAssignableFrom(serviceDescriptor.ImplementationType)) return;

            self.Add(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, $"{nameof(IHasServiceItem)}{serviceDescriptor.ServiceKey}", serviceDescriptor.ImplementationType, serviceDescriptor.Lifetime));
            self.Replace(ServiceDescriptor.Describe(serviceDescriptor.ServiceType, sp => GetImplementation(sp, self, serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey), serviceDescriptor.Lifetime));
        }

        void ConfigureKeyed()
        {
            if (!serviceDescriptor.IsKeyedService) return;

            if (!typeof(IHasServiceItem).IsAssignableFrom(serviceDescriptor.KeyedImplementationType)) return;

            self.Add(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, $"{nameof(IHasServiceItem)}{serviceDescriptor.ServiceKey}", serviceDescriptor.KeyedImplementationType, serviceDescriptor.Lifetime));
            self.Replace(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey, (sp, key) => GetImplementation(sp, self, serviceDescriptor.ServiceType, key), serviceDescriptor.Lifetime));
        }

        void ConfigureFactory()
        {
            if (!typeof(IHasServiceItem).IsAssignableFrom(serviceDescriptor.ServiceType)) return;

            self.Add(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, $"{nameof(IHasServiceItem)}{serviceDescriptor.ServiceKey}", (sp, key) => serviceDescriptor.ImplementationFactory?.Invoke(sp)!, serviceDescriptor.Lifetime));
            self.Replace(ServiceDescriptor.Describe(serviceDescriptor.ServiceType, sp => GetImplementation(sp, self, serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey), serviceDescriptor.Lifetime));
        }

        void ConfigureFactoryKeyed()
        {
            if (!serviceDescriptor.IsKeyedService) return;

            if (!typeof(IHasServiceItem).IsAssignableFrom(serviceDescriptor.ServiceType)) return;

            self.Add(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, $"{nameof(IHasServiceItem)}_{serviceDescriptor.ServiceKey}", (sp, key) => serviceDescriptor.KeyedImplementationFactory?.Invoke(sp, serviceDescriptor.ServiceKey)!, serviceDescriptor.Lifetime));
            self.Replace(ServiceDescriptor.DescribeKeyed(serviceDescriptor.ServiceType, serviceDescriptor.ServiceKey, (sp, key) => GetImplementation(sp, self, serviceDescriptor.ServiceType, key), serviceDescriptor.Lifetime));
        }

        void ConfigureSingletonInstance()
        {
            if (serviceDescriptor.IsKeyedService)
            {
                ConfigureInstance(serviceDescriptor.KeyedImplementationInstance, self);
                return;
            }

            ConfigureInstance(serviceDescriptor.ImplementationInstance, self);
        }

        Configure();
        ConfigureKeyed();
        ConfigureFactory();
        ConfigureFactoryKeyed();
        ConfigureSingletonInstance();

        self.TryAdd(serviceDescriptor);

        return self;
    }

    static void ConfigureInstance(object? instance, IServiceCollection serviceCollection, IServiceProvider? serviceProvider = null)
    {
        if (instance is null) return;

        if (instance is IHasServiceCollection hasServiceCollection)
            hasServiceCollection.ServiceCollection = serviceCollection;

        if (serviceProvider is not null && instance is IHasServiceProvider hasServiceProvider)
            hasServiceProvider.ServiceProvider = serviceProvider;
    }

    static object GetImplementation(IServiceProvider serviceProvider, IServiceCollection serviceCollection, Type serviceType, object? serviceKey)
    {
        var instance = serviceProvider.GetRequiredKeyedService(serviceType, $"{nameof(IHasServiceItem)}{serviceKey}");

        ConfigureInstance(instance, serviceCollection, serviceProvider);

        return instance;
    }
}