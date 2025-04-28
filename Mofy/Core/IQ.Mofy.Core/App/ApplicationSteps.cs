using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.Data.Annotations.DependencyInjection;
using IQ.Mofy.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

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

[ServiceTypes<IApplicationInitializeStep>]

public abstract class ApplicationInitializeStep<TApplication> : IApplicationInitializeStep where TApplication : IApplication
{
    public virtual Task OnInitializeAsync(IApplication application) => application is TApplication app ? OnInitializeAsync(app) : Task.CompletedTask;

    public abstract Task OnInitializeAsync(TApplication application);
}

[ServiceTypes<IApplicationPreRunStep>]

public abstract class ApplicationPreRunStep<TApplication> : IApplicationPreRunStep where TApplication : IApplication
{
    public virtual Task OnPreRunAsync(IApplication application) => application is TApplication app ? OnPreRunAsync(app) : Task.CompletedTask;

    public abstract Task OnPreRunAsync(TApplication application);
}

[ServiceTypes<IApplicationConfigureServicesStep>]

public abstract class ApplicationConfigureServicesStep<TApplication> : IApplicationConfigureServicesStep where TApplication : IApplication
{
    public Task OnConfigureServicesAsync(IServiceCollection services) => services.GetApplication() is TApplication app ? OnConfigureServicesAsync(app) : Task.CompletedTask;

    public abstract Task OnConfigureServicesAsync(TApplication application);
}

[ServiceTypes<IApplicationPostRunStep>]

public abstract class ApplicationPostRunStep<TApplication> : IApplicationPostRunStep where TApplication : IApplication
{
    public virtual Task OnPostRunAsync(IApplication application) => application is TApplication app ? OnPostRunAsync(app) : Task.CompletedTask;

    public abstract Task OnPostRunAsync(TApplication application);
}