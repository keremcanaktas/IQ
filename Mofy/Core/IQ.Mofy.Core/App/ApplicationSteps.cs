using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.Abstractions.App;
using Microsoft.Extensions.DependencyInjection;
using IQ.Mofy.Core.Data.Annotations.DependencyInjection;

namespace IQ.Mofy.Core.App;

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