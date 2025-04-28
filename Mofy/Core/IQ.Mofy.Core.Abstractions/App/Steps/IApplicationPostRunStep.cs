using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationPostRunStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnPostRunAsync(IApplication application);
}