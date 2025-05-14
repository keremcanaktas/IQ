using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Annotations;

namespace IQ.Mofy.Core.App.Steps;

[ServiceTypes<IApplicationPostRunStep>]

public abstract class ApplicationPostRunStep<TApplication> : IApplicationPostRunStep where TApplication : IApplication
{
    public virtual Task OnPostRunAsync(IApplication application) => application is TApplication app ? OnPostRunAsync(app) : Task.CompletedTask;

    public abstract Task OnPostRunAsync(TApplication application);
}