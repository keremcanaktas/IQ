using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Abstractions.Fundamentals.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public class ServiceProviderFactoryAdapter<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> serviceProviderFactory) : IServiceProviderFactory, IAdapter where TContainerBuilder : notnull
{
    public object CreateBuilder(IServiceCollection services) => serviceProviderFactory.CreateBuilder(services);
    
    public IServiceProvider CreateServiceProvider(object containerBuilder) => serviceProviderFactory.CreateServiceProvider((TContainerBuilder)containerBuilder);
}