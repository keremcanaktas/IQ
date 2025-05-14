using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.DependencyInjection.Services;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationInitializeStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnInitializeAsync(IApplication application);
}