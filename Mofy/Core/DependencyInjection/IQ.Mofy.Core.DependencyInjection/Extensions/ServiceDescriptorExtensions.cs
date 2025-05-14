using IQ.Mofy.Core.DependencyInjection.Descriptors;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace IQ.Mofy.Core.DependencyInjection.Extensions;

public static class ServiceDescriptorExtensions
{
    public static Type? GetImplementationType(this ServiceDescriptor? self)
    {
        if (self is null) return null!;

        if (self.ServiceKey is not null) return self.KeyedImplementationType ?? self.KeyedImplementationInstance?.GetType() ?? self.KeyedImplementationFactory?.GetType().GenericTypeArguments[2];

        return self.ImplementationType ?? self.ImplementationInstance?.GetType() ?? self.ImplementationFactory?.GetType().GenericTypeArguments[1];
    }

    public static object? GetImplementationInstance(this ServiceDescriptor? self)
    {
        if (self is null) return null!;

        return self.IsKeyedService ? self.KeyedImplementationInstance : self.ImplementationInstance;
    }

    public static object? ProduceImplementationInstance(this ServiceDescriptor? self)
    {
        if (self?.ImplementationInstance is not null)
            return self.ImplementationInstance;

        if (typeof(ISingletonInstance).IsAssignableFrom(self?.GetImplementationType()))
            return self.ImplementationFactory?.Invoke(default!);

        return null;
    }
}