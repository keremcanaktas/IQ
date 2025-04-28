// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.App;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;

var application = new WebApiApplication();

application.ServiceCollection.AddRegify();

await application.RunAsync();



public class ApplicationPostRunStep : ApplicationPostRunStep<WebApiApplication>
{
    public override Task OnPostRunAsync(WebApiApplication apiApplication)
    {
        apiApplication.MapGet("/", async (ICarRepository carRepository) => await carRepository.GetListAsync(d => true));


        return Task.CompletedTask;
    }
}