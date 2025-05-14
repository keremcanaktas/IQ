using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Annotations;

namespace IQ.Mofy.Core.App.Steps;

[ServiceTypes<IApplicationInitializeStep>]

public abstract class ApplicationInitializeStep<TApplication> : IApplicationInitializeStep where TApplication : IApplication
{
    public virtual Task OnInitializeAsync(IApplication application) => application is TApplication app ? OnInitializeAsync(app) : Task.CompletedTask;

    public abstract Task OnInitializeAsync(TApplication application);
}
