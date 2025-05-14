using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global

namespace IQ.Mofy.Core.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static T? GetService<T>(this IServiceCollection self) => self.GetService(typeof(T)) is T instance ? instance : default;

    public static object? GetService(this IServiceCollection self, Type serviceType)
    {
        var services = serviceType switch
        {
            { IsGenericType: true } => self.GetServices(sd => sd.ServiceType.IsGenericType && sd.ServiceType.GetGenericTypeDefinition() == serviceType.GetGenericTypeDefinition()),
            _ => self.GetServices(serviceType)
        };

        return services.FirstOrDefault();
    }

    public static T GetRequiredService<T>(this IServiceCollection self) => (T)self.GetRequiredService(typeof(T));

    public static object GetRequiredService(this IServiceCollection self, Type serviceType) => self.GetService(serviceType) ?? throw new InvalidOperationException($"No service for type '{serviceType.Name}' has been registered.");


    public static IEnumerable<T> GetServices<T>(this IServiceCollection self) => self.GetServices(typeof(T)).Cast<T>();

    public static ICollection<T> GetServiceCollection<T>(this IServiceCollection self) => [.. self.GetServices<T>()];

    public static IEnumerable<object?> GetServices(this IServiceCollection self, Type serviceType) => self.GetServices(sd => sd.ServiceType == serviceType);

    public static IEnumerable<object?> GetServices(this IServiceCollection self, Func<ServiceDescriptor, bool> predicate) => self.Where(predicate).Select(serviceDescriptor => serviceDescriptor.ProduceImplementationInstance());
}