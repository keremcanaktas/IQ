using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace IQ.Mofy.Core.Extensions;

public static class ServiceProviderExtensions
{
    public static IServiceProvider AddHandler(this IServiceProvider provider, Action<IServiceProvider, Type, object?> handler)
    {
        var field = typeof(ServiceProvider).GetField("_createServiceAccessor", BindingFlags.Instance | BindingFlags.NonPublic);
        var accessor = (Delegate?)field?.GetValue(provider);


        var newAccessor = (Type type) => (object scope) =>
        {
            var resolver = accessor?.DynamicInvoke([type]) as Delegate;
            var resolved = resolver?.DynamicInvoke([scope]);

            handler(provider, type, resolved);

            return resolved;
        };

        field?.SetValue(provider, newAccessor);
        return provider;
    }
}