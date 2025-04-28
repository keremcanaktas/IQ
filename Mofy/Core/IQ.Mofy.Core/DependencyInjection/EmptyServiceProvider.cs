
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public interface IEmptyServiceProvider : IServiceProvider, ISupportRequiredService;

public class EmptyServiceProvider : IEmptyServiceProvider
{
    public static EmptyServiceProvider Instance { get; } = new EmptyServiceProvider();

    public object GetRequiredService(Type serviceType) => default!;

    public object? GetService(Type serviceType) => default;
}