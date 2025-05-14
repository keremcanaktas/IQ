using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Annotations;

namespace IQ.Mofy.Core.App.Steps;

[ServiceTypes<IApplicationPreRunStep>]

public abstract class ApplicationPreRunStep<TApplication> : IApplicationPreRunStep where TApplication : IApplication
{
    public virtual Task OnPreRunAsync(IApplication application) => application is TApplication app ? OnPreRunAsync(app) : Task.CompletedTask;

    public abstract Task OnPreRunAsync(TApplication application);
}
