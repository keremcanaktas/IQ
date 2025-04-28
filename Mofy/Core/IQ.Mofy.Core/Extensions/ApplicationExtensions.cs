using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;
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

    public static IApplication AddStep(this IApplication self, IApplicationStep applicationStep)
    {
        if (applicationStep is IApplicationInitializeStep applicationInitializeStep) self.ServiceCollection.AddSingleton(applicationInitializeStep);

        if(applicationStep is IApplicationPreRunStep applicationPreRunStep) self.ServiceCollection.AddSingleton(applicationPreRunStep);

        if (applicationStep is IApplicationConfigureServicesStep applicationConfigureServicesStep) self.ServiceCollection.AddSingleton(applicationConfigureServicesStep);

        if (applicationStep is IApplicationPostRunStep applicationPostRunStep) self.ServiceCollection.AddSingleton(applicationPostRunStep);

        return self;
    }
}