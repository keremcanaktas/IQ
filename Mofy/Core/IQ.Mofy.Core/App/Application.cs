using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Accessors;
using IQ.Mofy.Core.DependencyInjection.Adapters;
using IQ.Mofy.Core.DependencyInjection.Extensions;
using IQ.Mofy.Core.Fundamentals.Disposable;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable VirtualMemberCallInConstructor

namespace IQ.Mofy.Core.App;

public class Application : AsyncDisposable,
    IApplication,
    IServiceCollectionAccessor,
    IServiceProviderAccessor
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

        await CreateServiceProviderAsync();

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

    protected virtual Task CreateServiceProviderAsync()
    {
        var serviceProviderFactory = ServiceProviderFactoryAdapter.CreateOrDefault(ServiceCollection.GetService(typeof(IServiceProviderFactory<>)));

        ServiceProvider = serviceProviderFactory.CreateServiceProvider(serviceProviderFactory.CreateBuilder(ServiceCollection));

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