using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection.Decorators;

public class ServiceProviderFactoryDecorator<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory) : IServiceProviderFactory<TContainerBuilder>
    where TContainerBuilder : notnull
{
    public virtual TContainerBuilder CreateBuilder(IServiceCollection services) => serviceProviderFactory.CreateBuilder(services);

    public virtual IServiceProvider CreateServiceProvider(TContainerBuilder containerBuilder) => serviceProviderFactory.CreateServiceProvider(containerBuilder);
}