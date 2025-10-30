
using IQ.Mofy.Core.DependencyInjection.Accessors;
using IQ.Mofy.Core.DependencyInjection.Decorators;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Reflection;
using static System.GC;

namespace IQ.Mofy.Core.DependencyInjection.Providers;

public interface IServiceProviderWrapper;

[DebuggerDisplay("{DebuggerToString(),nq}")]
[DebuggerTypeProxy(typeof(ServiceProviderDebugView))]
public class ServiceProvider(IServiceProvider inner) : IKeyedServiceProvider, IServiceScopeFactory, IDisposable, IAsyncDisposable, IServiceProviderWrapper
{
    #region Ctor

    public ServiceProvider(IServiceProvider inner, bool isScope) : this(inner) => IsScope = isScope;

    #endregion

    #region Properties

    protected bool IsScope { get; init; }

    public bool Disposed { get; protected set; }

    #endregion

    #region IServiceProvider

    public object? GetService(Type serviceType)
    {
        if (typeof(IServiceProvider).IsAssignableFrom(serviceType)
            || typeof(IServiceScopeFactory).IsAssignableFrom(serviceType)) return this;

        var service = inner.GetService(serviceType);

        ApplyAccessors(service);
        service = ApplyDecorators(service);

        return service;
    }

    #endregion

    #region IKeyedServiceProvider

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (inner is not IKeyedServiceProvider keyedServiceProvider) throw new InvalidOperationException("This service descriptor is keyed. Your service provider may not support keyed services.");

        var service = keyedServiceProvider.GetKeyedService(serviceType, serviceKey);

        ApplyAccessors(service);
        service = ApplyDecorators(service);

        return service;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (inner is not IKeyedServiceProvider keyedServiceProvider) throw new InvalidOperationException("This service descriptor is keyed. Your service provider may not support keyed services.");

        var service = keyedServiceProvider.GetRequiredKeyedService(serviceType, serviceKey);

        ApplyAccessors(service);
        service = ApplyDecorators(service);

        return service!;
    }

    #endregion

    #region IServiceScopeFactory

    public IServiceScope CreateScope() => new ServiceScope(inner.CreateScope());

    #endregion

    #region Applys

    private void ApplyAccessors(object? service)
    {
        if (service is IServiceProviderAccessor serviceProviderAccessor)
            serviceProviderAccessor.ServiceProvider = this;

        if (service is IServiceCollectionAccessor serviceCollectionAccessor)
            serviceCollectionAccessor.ServiceCollection = inner.GetRequiredService<IServiceCollection>();
    }

    private object? ApplyDecorators(object? service)
    {
        if (service is null) return service;

        foreach (var decorator in inner.GetServices<IServiceDecorator>())
            service = decorator.Decorate(this, service);

        return service;
    }

    #endregion

    #region Debugger

    protected string DebuggerToString()
    {
        return inner.GetType().GetMethod(nameof(DebuggerToString), BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(inner, [])?.ToString() ?? DefaultString();

        string DefaultString()
        {
            var debugText = $"ServiceDescriptors = {GetServiceDescriptors().Count()}";
            if (!IsScope) debugText += ", IsScope = true";
            if (Disposed) debugText += ", Disposed = true";
            return debugText;
        }
    }

    protected IEnumerable<ServiceDescriptor> GetServiceDescriptors()
    {
        var root = inner.GetType().GetProperty("Root", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(inner) ?? inner;

        var rootProvider = root.GetType().GetProperty("RootProvider", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(root);

        var callSiteFactory = rootProvider?.GetType().GetProperty("CallSiteFactory", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(rootProvider);

        var descriptors = callSiteFactory?.GetType().GetProperty("Descriptors", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(callSiteFactory) as IEnumerable<ServiceDescriptor>;

        return descriptors ?? [];
    }

    protected IEnumerable<object> GetDisposables()
    {
        var root = inner.GetType().GetProperty("Root", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(inner) ?? inner;

        var disposables = root.GetType().GetProperty("Disposables", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(root) as IEnumerable<object>;

        return disposables ?? [];
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        SuppressFinalize(this);
    }

    public void Dispose(bool disposing)
    {
        if (Disposed) return;

        Disposed = true;

        if (!disposing) return;

        if (inner is IDisposable disposable) disposable.Dispose();
    }

    #endregion

    #region IAsyncDisposable

    public async ValueTask DisposeAsync()
    {
        if (Disposed) return;

        Disposed = true;

        if (inner is IAsyncDisposable asyncDisposable) await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        else (inner as IDisposable)?.Dispose();

        SuppressFinalize(this);
    }

    #endregion

    #region DebugView

    internal sealed class ServiceProviderDebugView(ServiceProvider serviceProvider)
    {
        public List<ServiceDescriptor> ServiceDescriptors => [.. serviceProvider.GetServiceDescriptors()];
        public List<object> Disposables => [.. serviceProvider.GetDisposables()];
        public bool Disposed => serviceProvider.Disposed;
        public bool IsScope => !serviceProvider.IsScope;
    }

    #endregion
}

[DebuggerDisplay("{DebuggerToString(),nq}")]
internal class ServiceScope(IServiceScope innerScope) : IServiceScope, IAsyncDisposable
{
    private bool _disposed = false;

    public IServiceProvider ServiceProvider { get; } = new ServiceProvider(innerScope.ServiceProvider, true);

    public void Dispose()
    {
        Dispose(true);
        SuppressFinalize(this);
    }

    protected void Dispose(bool disposing)
    {
        if (_disposed) return;

        _disposed = true;

        if (disposing)
            innerScope.Dispose();
    }


    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (innerScope is IAsyncDisposable asyncDisposable) await asyncDisposable.DisposeAsync();
        else innerScope.Dispose();

        SuppressFinalize(this);
    }

    #region Debugger

    private string? DebuggerToString() => innerScope.GetType().GetMethod(nameof(DebuggerToString), BindingFlags.Instance | BindingFlags.NonPublic)?.Invoke(innerScope, [])?.ToString();

    #endregion
}