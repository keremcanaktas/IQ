using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Annotations;
using IQ.Mofy.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.App.Steps;

[ServiceTypes<IApplicationConfigureServicesStep>]

public abstract class ApplicationConfigureServicesStep<TApplication> : IApplicationConfigureServicesStep where TApplication : IApplication
{
    public Task OnConfigureServicesAsync(IServiceCollection services) => services.GetApplication() is TApplication app ? OnConfigureServicesAsync(app) : Task.CompletedTask;

    public abstract Task OnConfigureServicesAsync(TApplication application);
}
