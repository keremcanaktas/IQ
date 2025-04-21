using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Abstractions.Fundamentals.Steps;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationPostRunStep : IStep, IHasSingletonInstance
{
    public Task OnPostRunAsync(IApplication application);
}