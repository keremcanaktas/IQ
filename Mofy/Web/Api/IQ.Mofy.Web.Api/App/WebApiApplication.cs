using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using WebApplication = IQ.Mofy.Web.App.WebApplication;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
// ReSharper disable MemberCanBeProtected.Global

namespace IQ.Mofy.Web.Api.App;

public class WebApiApplication(WebApplicationBuilder webApplicationBuilder) : WebApplication(webApplicationBuilder.Services), IHost, IApplicationBuilder, IEndpointRouteBuilder
{
    public WebApiApplication(string[] args) : this(Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args)) { }
    public WebApiApplication() : this([]) { }

    #region Host

    public WebApplicationBuilder HostApplicationBuilder { get; } = webApplicationBuilder;

    public Microsoft.AspNetCore.Builder.WebApplication? HostApplication { get; protected set; }

    #endregion

    #region IApplication

    protected override Task OnInitializingAsync()
    {
        ServiceCollection.AddSingleton<IConfiguration>(HostApplicationBuilder.Configuration);
        return base.OnInitializingAsync();
    }

    protected override Task<IServiceProvider> CreateServiceProviderAsync(IServiceProviderFactory<object> factory)
    {
        HostApplicationBuilder.Host.UseServiceProviderFactory(factory);

        HostApplication ??= HostApplicationBuilder.Build();

        return Task.FromResult(HostApplication.Services);
    }

    #endregion

    #region Run

    public virtual Task BuildHostApplicationAsync() => base.RunAsync();

    public override Task RunAsync() => RunAsync(null);

    public virtual async Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? url)
    {
        if (HostApplication is null)
            await BuildHostApplicationAsync();

        await HostApplication!.RunAsync(url);
    }

    #endregion

    #region IHost

    public IServiceProvider Services => HostApplication!.Services;

    Task IHost.StartAsync(CancellationToken cancellationToken) => HostApplication!.StartAsync(cancellationToken);

    Task IHost.StopAsync(CancellationToken cancellationToken) => HostApplication!.StopAsync(cancellationToken);

    #endregion

    #region IApplicationBuilder

    private IApplicationBuilder ApplicationBuilder => HostApplication!;

    IServiceProvider IApplicationBuilder.ApplicationServices
    {
        get => ApplicationBuilder.ApplicationServices;
        set => ApplicationBuilder.ApplicationServices = value;
    }

    IFeatureCollection IApplicationBuilder.ServerFeatures => ApplicationBuilder.ServerFeatures;

    IDictionary<string, object?> IApplicationBuilder.Properties => ApplicationBuilder.Properties;

    IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware) => HostApplication!.Use(middleware);

    IApplicationBuilder IApplicationBuilder.New() => ApplicationBuilder.New();

    RequestDelegate IApplicationBuilder.Build() => ApplicationBuilder.Build();

    #endregion

    #region IEndpointRouteBuilder

    private IEndpointRouteBuilder EndpointRouteBuilder => HostApplication!;

    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => EndpointRouteBuilder.CreateApplicationBuilder();

    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => EndpointRouteBuilder.DataSources;

    #endregion
}