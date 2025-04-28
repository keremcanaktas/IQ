// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.App;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var application = new WebApiApplication(WebApplication.CreateSlimBuilder(args));

application.ServiceCollection.AddRegify();

application.HostApplicationBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { { "Mofy:AnnotationOptions:Key", "Test" } });

await application.RunAsync();


public class ApplicationConfigurator : IApplicationConfigureServicesStep
{
    public Task OnConfigureServicesAsync(IServiceCollection services)
    {
        services.Configure<AnnotationOptions>(key: nameof(IQ.Mofy));

        return Task.CompletedTask;
    }
}



public class ApplicationPostRunStep : IApplicationPostRunStep
{
    public Task OnPostRunAsync(IApplication application)
    {
        (application as WebApiApplication)?.MapGet("/", async (IOptions<AnnotationOptions> applicationOptions, ICarRepository carRepository) => await carRepository.GetListAsync(d => true));

        return Task.CompletedTask;
    }
}


class ApplicationOptions
{
    public string Name { get; set; }
}

public class AnnotationOptions
{
    public string? Key { get; set; }
}