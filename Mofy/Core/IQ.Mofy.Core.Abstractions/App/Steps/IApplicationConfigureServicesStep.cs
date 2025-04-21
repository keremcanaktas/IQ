using IQ.Mofy.Core.Abstractions.DependencyInjection;
using IQ.Mofy.Core.Abstractions.Fundamentals.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationConfigureServicesStep : IStep, IHasSingletonInstance
{
    public Task OnConfigureServicesAsync(IServiceCollection services);
}