using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationConfigureServicesStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnConfigureServicesAsync(IServiceCollection services);
}