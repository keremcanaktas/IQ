using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.DependencyInjection.Services;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationPreRunStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnPreRunAsync(IApplication application);
}