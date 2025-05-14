using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace IQ.Mofy.Core.DependencyInjection.Adapters;

public class ServiceProviderFactoryAdapter
{
    public static IServiceProviderFactory<object> CreateOrDefault(object? serviceProviderFactory)
    {
        if (serviceProviderFactory is null) return new ServiceProviderFactoryAdapter<IServiceCollection>(new DefaultServiceProviderFactory());

        return (IServiceProviderFactory<object>)Activator.CreateInstance(typeof(ServiceProviderFactoryAdapter<>).MakeGenericType(GetBuilderType(serviceProviderFactory.GetType())!), [serviceProviderFactory])!;
    }

    private static Type? GetBuilderType(Type serviceProviderFactoryType)
    {
        return serviceProviderFactoryType.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IServiceProviderFactory<>)).SelectMany(i => i.GetGenericArguments()).FirstOrDefault();
    }
}

[DebuggerStepThrough]
public class ServiceProviderFactoryAdapter<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory) : ServiceProviderFactoryAdapter, IServiceProviderFactory<object> where TContainerBuilder : notnull
{
    public object CreateBuilder(IServiceCollection services) => serviceProviderFactory.CreateBuilder(services);

    public IServiceProvider CreateServiceProvider(object containerBuilder) => serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
}