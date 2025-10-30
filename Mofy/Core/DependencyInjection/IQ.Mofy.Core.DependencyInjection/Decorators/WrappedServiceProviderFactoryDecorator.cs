using Microsoft.Extensions.DependencyInjection;
using ServiceProvider = IQ.Mofy.Core.DependencyInjection.Providers.ServiceProvider;

namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public class WrappedServiceProviderFactoryDecorator<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory) : ServiceProviderFactoryDecorator<TContainerBuilder>(serviceProviderFactory)
    where TContainerBuilder : notnull
{
    public override IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder)
    {
        var serviceProvider = base.CreateServiceProvider(containerBuilder);

        return new ServiceProvider(serviceProvider);
    }
}