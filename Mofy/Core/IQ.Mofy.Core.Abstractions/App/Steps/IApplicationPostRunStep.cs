using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.DependencyInjection.Services;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationPostRunStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnPostRunAsync(IApplication application);
}