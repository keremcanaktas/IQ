using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Extensions;

public static class ApplicationExtensions
{
    public static IApplication UseServiceProviderFactory<TContainerBuilder>(this IApplication self, IServiceProviderFactory<TContainerBuilder> serviceProviderFactory) where TContainerBuilder : notnull
    {
        self.ServiceCollection.AddSingleton<IServiceProviderFactory>(new ServiceProviderFactoryAdapter<TContainerBuilder>(serviceProviderFactory));
        return self;
    }
}