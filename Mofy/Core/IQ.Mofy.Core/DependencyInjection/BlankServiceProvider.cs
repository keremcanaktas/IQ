using IQ.Mofy.Core.Abstractions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public class BlankServiceProvider : IBlankServiceProvider
{
    public static BlankServiceProvider Instance { get; } = new();

    public object GetRequiredService(Type serviceType) => null!;

    public object? GetService(Type serviceType) => null;
}