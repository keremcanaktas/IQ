using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.DependencyInjection.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.App.Steps;

public interface IApplicationConfigureServicesStep : IApplicationStep, ISingletonInstance, IRequiredService
{
    public Task OnConfigureServicesAsync(IServiceCollection services);
}