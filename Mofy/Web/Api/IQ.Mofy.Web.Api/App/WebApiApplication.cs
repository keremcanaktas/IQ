using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
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

    public WebApplicationBuilder HostBuilder { get; } = webApplicationBuilder;

    public Microsoft.AspNetCore.Builder.WebApplication Host { get; protected set; } = null!;

    #endregion

    #region IApplication

    protected override Task CreateServiceProviderAsync()
    {
        Host ??= HostBuilder.Build();
        ServiceProvider = Host.Services;
        return Task.CompletedTask;
    }

    #endregion

    public virtual Task BuildHostAsync() => base.RunAsync();

    public override Task RunAsync() => RunAsync(null);
    
    public virtual async Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? url) => await Host.RunAsync(url);

    #region IHost

    public IServiceProvider Services => Host.Services;
    
    Task IHost.StartAsync(CancellationToken cancellationToken) => Host.StartAsync(cancellationToken);

    Task IHost.StopAsync(CancellationToken cancellationToken) => Host.StopAsync(cancellationToken);

    #endregion
    
    #region IApplicationBuilder

    private IApplicationBuilder ApplicationBuilder => Host;
    
    IServiceProvider IApplicationBuilder.ApplicationServices
    {
        get => ApplicationBuilder.ApplicationServices;
        set => ApplicationBuilder.ApplicationServices = value;
    }
    
    IFeatureCollection IApplicationBuilder.ServerFeatures => ApplicationBuilder.ServerFeatures;
    
    IDictionary<string, object?> IApplicationBuilder.Properties => ApplicationBuilder.Properties;
    
    IApplicationBuilder IApplicationBuilder.Use(Func<RequestDelegate, RequestDelegate> middleware) => Host.Use(middleware);

    IApplicationBuilder IApplicationBuilder.New() => ApplicationBuilder.New();

    RequestDelegate IApplicationBuilder.Build() => ApplicationBuilder.Build();

    #endregion
    
    #region IEndpointRouteBuilder
    
    private IEndpointRouteBuilder EndpointRouteBuilder => Host;
    
    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => EndpointRouteBuilder.CreateApplicationBuilder(); 

    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => EndpointRouteBuilder.DataSources;
    
    #endregion
}