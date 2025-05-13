using IQ.Mofy.Core.Abstractions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public class ServiceProviderHandler : IServiceProviderHandler
{
    public void Handle(IServiceProvider serviceProvider, Type type, object? serviceKey, object? instance)
    {
        if (instance is IHasServiceCollection hasServiceCollection)
            hasServiceCollection.ServiceCollection = serviceProvider.GetService<IServiceCollection>()!;

        if (serviceProvider is IBlankServiceProvider) return;

        if (instance is IHasServiceProvider hasServiceProvider)
            hasServiceProvider.ServiceProvider = serviceProvider;


        if (type == typeof(IEnumerable<IServiceCollectionItemDecorator>)) return;

        foreach (var serviceItemDecorator in serviceProvider.GetServices<IServiceCollectionItemDecorator>())
            serviceItemDecorator.Decorate(serviceProvider, instance);
    }
}