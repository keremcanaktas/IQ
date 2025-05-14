using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.App.Steps;

[ServiceTypes<IApplicationInitializeStep>]
[ServiceTypes<IApplicationPreRunStep>]
[ServiceTypes<IApplicationConfigureServicesStep>]
[ServiceTypes<IApplicationPostRunStep>]

public abstract class ApplicationSteps : IApplicationInitializeStep, IApplicationPreRunStep, IApplicationConfigureServicesStep, IApplicationPostRunStep
{
    public virtual Task OnInitializeAsync(IApplication application) => Task.CompletedTask;

    public virtual Task OnPreRunAsync(IApplication application) => Task.CompletedTask;

    public virtual Task OnConfigureServicesAsync(IServiceCollection services) => Task.CompletedTask;

    public virtual Task OnPostRunAsync(IApplication application) => Task.CompletedTask;
}