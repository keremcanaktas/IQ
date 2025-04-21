using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.App;

public interface IApplicationCreator<out T>
{
    public static abstract T Create(Type startupType, IServiceCollection? services = null);
    public static abstract T Create(IServiceCollection? services = null);
    public static abstract T Create<TStartup>(IServiceCollection? services = null);
}