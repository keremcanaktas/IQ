using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Extensions;

public static class ServiceDescriptorExtensions
{
    public static Type? GetImplementationType(this ServiceDescriptor? self)
    {
        if (self is null) return default!;

        if (self.ServiceKey is not null) return self.KeyedImplementationType ?? self.KeyedImplementationInstance?.GetType() ?? self.KeyedImplementationFactory?.GetType().GenericTypeArguments[2];

        return self.ImplementationType ?? self.ImplementationInstance?.GetType() ?? self.ImplementationFactory?.GetType().GenericTypeArguments[1];
    }

    public static ServiceDescriptor DescribeWithKey(this ServiceDescriptor self)
    {
        var serviceDescriptorKey = new ServiceDescriptorKey(self.ServiceType, self.ServiceKey)
        {
            Factory = self.ImplementationFactory,
            KeyedFactory = self.IsKeyedService ? self.KeyedImplementationFactory : default
        };

        if (self.ImplementationType is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                implementationType: self.ImplementationType,
                lifetime: self.Lifetime,
                serviceKey: serviceDescriptorKey);

        if (self.ImplementationInstance is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                instance: self.ImplementationInstance,
                serviceKey: serviceDescriptorKey);

        if (self.ImplementationFactory is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                factory: (sp, k) => (k as ServiceDescriptorKey?)?.Factory?.Invoke(sp)!,
                lifetime: self.Lifetime,
                serviceKey: serviceDescriptorKey);

        if (self.KeyedImplementationType is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                implementationType: self.KeyedImplementationType,
                lifetime: self.Lifetime,
                serviceKey: serviceDescriptorKey);

        if (self.KeyedImplementationInstance is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                instance: self.KeyedImplementationInstance,
                serviceKey: serviceDescriptorKey);

        if (self.KeyedImplementationFactory is not null)
            return new ServiceDescriptor(
                serviceType: self.ServiceType,
                factory: (sp, k) => k is ServiceDescriptorKey serviceItemKey ? serviceItemKey.KeyedFactory?.Invoke(sp, serviceItemKey.Key)! : default!,
                lifetime: self.Lifetime,
                serviceKey: serviceDescriptorKey);

        return self;
    }
}

public readonly struct ServiceDescriptorKey(Type serviceType, object? key)
{
    public Type ServiceType { get; } = serviceType;
    public object? Key { get; } = key;

    public Func<IServiceProvider, object>? Factory { get; init; } = null;
    public Func<IServiceProvider, object?, object>? KeyedFactory { get; init; } = null;


    public override bool Equals(object? obj) => obj is ServiceDescriptorKey other && ServiceType == other.ServiceType && Key == other.Key;

    private bool Equals(ServiceDescriptorKey other) => ServiceType == other.ServiceType && Key == other.Key;

    public override int GetHashCode() => HashCode.Combine(ServiceType, Key);

    public static bool operator ==(ServiceDescriptorKey left, ServiceDescriptorKey right) => left.Equals(right);

    public static bool operator !=(ServiceDescriptorKey left, ServiceDescriptorKey right) => !(left == right);
}