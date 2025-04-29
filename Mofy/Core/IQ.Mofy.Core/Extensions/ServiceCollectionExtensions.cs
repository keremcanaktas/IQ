using IQ.Mofy.Core.Abstractions.App;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global

namespace IQ.Mofy.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static T? GetService<T>(this IServiceCollection self) => self.GetService(typeof(T)) is T t ? t : default;

    public static object? GetService(this IServiceCollection self, Type type) => self.GetServices(type).FirstOrDefault();


    public static T GetRequiredService<T>(this IServiceCollection self) => (T)self.GetRequiredService(typeof(T));

    public static object GetRequiredService(this IServiceCollection self, Type type) => self.GetService(type) ?? throw new InvalidOperationException($"No service for type '{type.Name}' has been registered.");


    public static IEnumerable<T> GetServices<T>(this IServiceCollection self) => self.GetServices(typeof(T)).Cast<T>();

    public static ICollection<T> GetServiceCollection<T>(this IServiceCollection self) => [.. self.GetServices<T>()];

    public static IEnumerable<object?> GetServices(this IServiceCollection self, Type type) => self.Where(s => s.ServiceType == type).Select(serviceDescriptor => serviceDescriptor.ProduceImplementationInstance());


    public static IApplication GetApplication(this IServiceCollection self) => self.GetRequiredService<IApplication>();
}