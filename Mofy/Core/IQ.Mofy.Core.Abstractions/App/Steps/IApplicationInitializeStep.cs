using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationInitializeStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnInitializeAsync(IApplication application);
}