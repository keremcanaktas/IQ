using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Adapters;
using IQ.Mofy.Core.DependencyInjection.Decorators;
using IQ.Mofy.Core.DependencyInjection.Extensions;
using IQ.Mofy.Core.Fundamentals.Disposable;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable VirtualMemberCallInConstructor

namespace IQ.Mofy.Core.App;

public class Application : AsyncDisposable, IApplication
{
    #region Ctor

    public Application(IServiceCollection serviceCollection)
    {
        ServiceCollection = serviceCollection;
        OnInitializeAsync();
    }

    public Application() : this(new ServiceCollection()) { }

    #endregion

    #region IServiceCollectionAccessor

    public virtual IServiceCollection ServiceCollection { get; set; } = null!;

    #endregion

    #region IServiceProviderAccessor

    public virtual IServiceProvider ServiceProvider { get; set; } = null!;

    #endregion

    #region IApplication

    public IApplicationOptions Options { get; set; } = new ApplicationOptions();

    public virtual async Task RunAsync()
    {
        await OnPreRunAsync();

        await OnConfigureServicesAsync();

        var serviceProviderFactory = await CreateServiceProviderFactoryAsync();

        var serviceProvider = await CreateServiceProviderAsync(serviceProviderFactory);

        await ApplyServiceProviderAsync(serviceProvider);

        await OnPostRunAsync();
    }

    public virtual Task StopAsync()
    {
        ServiceCollection?.Clear();
        ServiceCollection = null!;
        ServiceProvider = null!;

        return Task.CompletedTask;
    }

    #endregion

    #region DependencyInjection

    protected virtual Task<IServiceProviderFactory<object>> CreateServiceProviderFactoryAsync() => Task.FromResult<IServiceProviderFactory<object>>(new WrappedServiceProviderFactoryDecorator<object>(ServiceProviderFactoryAdapter.CreateOrDefault(ServiceCollection.GetService(typeof(IServiceProviderFactory<>)))));

    protected virtual Task<IServiceProvider> CreateServiceProviderAsync(IServiceProviderFactory<object> serviceProviderFactory) => Task.FromResult(serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(ServiceCollection)));

    protected virtual Task ApplyServiceProviderAsync(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        return Task.CompletedTask;
    }

    #endregion

    #region Steps

    private async void OnInitializeAsync()
    {
        try
        {
            ServiceCollection.AddSingleton<IApplication>(this);
            ServiceCollection.AddSingleton(ServiceCollection);

            await OnInitializingAsync();

            var tasks = ServiceCollection.GetServiceCollection<IApplicationInitializeStep>().Select(i => i.OnInitializeAsync(this));
            await Task.WhenAll(tasks);

            await OnInitializedAsync();
        }
        catch (Exception exception)
        {
            Debug.WriteLine(exception);
        }
    }

    protected virtual Task OnInitializingAsync() => Task.CompletedTask;

    protected virtual Task OnInitializedAsync() => Task.CompletedTask;


    protected virtual Task OnPreRunAsync()
    {
        var tasks = ServiceCollection.GetServiceCollection<IApplicationPreRunStep>().Select(s => s.OnPreRunAsync(this));
        return Task.WhenAll(tasks);
    }

    protected virtual Task OnConfigureServicesAsync()
    {
        var tasks = ServiceCollection.GetServiceCollection<IApplicationConfigureServicesStep>().Select(s => s.OnConfigureServicesAsync(ServiceCollection));
        return Task.WhenAll(tasks);
    }

    protected virtual Task OnPostRunAsync()
    {
        var tasks = ServiceCollection.GetServiceCollection<IApplicationPostRunStep>().Select(s => s.OnPostRunAsync(this));
        return Task.WhenAll(tasks);
    }

    #endregion

    #region Disposable

    protected override void ReleaseManagedResources()
    {
        StopAsync()
            .ConfigureAwait(false);
        base.ReleaseManagedResources();
    }

    #endregion

    #region AsyncDisposable

    protected override async ValueTask ReleaseManagedResourcesAsync()
    {
        await StopAsync();
        await base.ReleaseManagedResourcesAsync();
    }

    #endregion
}