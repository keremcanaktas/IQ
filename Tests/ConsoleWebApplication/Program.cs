// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.App;
using IQ.Mofy.Core.Extensions;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var application = new WebApiApplication(WebApplication.CreateSlimBuilder(args));

application.ServiceCollection.AddRegify();

application.HostApplicationBuilder.Configuration.AddInMemoryCollection();

await application.RunAsync();



public class ApplicationConfigurator : IApplicationConfigureServicesStep
{
    public Task OnConfigureServicesAsync(IServiceCollection services)
    {
        var configuration = services.GetRequiredService<IConfiguration>();

        services.Configure<ApplicationOptions>(configuration.GetSection(nameof(ApplicationOptions)));

        return Task.CompletedTask;
    }
}



public class CarService : IApplicationPostRunStep
{
    public Task OnPostRunAsync(IApplication application)
    {
        (application as WebApiApplication)?
            .MapGet("/GetCarList", async (IOptions<ApplicationOptions> applicationOptions, ICarRepository carRepository) =>
            {
                return await carRepository.GetListAsync(d => true);
            });



        return Task.CompletedTask;
    }
}


class ApplicationOptions
{
    public string Name { get; set; }
}