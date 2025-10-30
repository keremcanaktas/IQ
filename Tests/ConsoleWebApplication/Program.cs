// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.App.Steps;
using IQ.Mofy.Core.DependencyInjection.Decorators;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var application = new WebApiApplication();

application.ServiceCollection.AddRegify();

var httpContextFactoryDescriptor = application.ServiceCollection.FirstOrDefault(t => t.ServiceType == typeof(IHttpContextFactory));


await application.BuildHostApplicationAsync();

var hostedServiceDescriptor = application.ServiceCollection.FirstOrDefault(t => t.ServiceType == typeof(IHostedService));

await application.RunAsync();

public class ApplicationPostRunStep : ApplicationPostRunStep<WebApiApplication>
{
    public ApplicationPostRunStep()
    {
        Console.Out.Write("Created");
    }

    public override Task OnPostRunAsync(WebApiApplication apiApplication)
    {
        var hostedServices = apiApplication.ServiceProvider.GetServices<IHostedService>();

        var host = apiApplication.ServiceProvider.GetRequiredService<IHost>();

        var b = host.GetType();

        var httpContextFactory = apiApplication.Services.GetService<IHttpContextFactory>();

        var hostedService = apiApplication.ServiceProvider.GetRequiredService<IHostedService>();

        apiApplication.MapGet("/", (IServiceProvider? serviceScopeFactory) =>
        {
            var scope = serviceScopeFactory.CreateScope();

            return "test";

            //var t = context.RequestServices.GetType();

            //var c = applicationPostRunStep == this;

            //return applicationPostRunStep.GetType().Name;
        });



        //ServiceProvider


        return Task.CompletedTask;
    }
}


public class ContextAccessorDecorator : ServiceDecorator<IContextAccessor>
{
    public override IContextAccessor Decorate(IServiceProvider serviceProvider, IContextAccessor service)
    {
        service.Context = serviceProvider.GetRequiredService<IContext>();

        return service;
    }
}