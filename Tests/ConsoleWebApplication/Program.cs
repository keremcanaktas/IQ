// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Abstractions.App.Steps;
using IQ.Mofy.Core.App;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;

var application = new WebApiApplication();

application.ServiceCollection.AddRegify();

await application.RunAsync();



public class CarService : IApplicationPostRunStep
{
    public Task OnPostRunAsync(IApplication application)
    {
        (application as WebApiApplication)?
            .MapGet("/GetCarList", async (ICarRepository carRepository) => await carRepository.GetListAsync(d => true));



        return Task.CompletedTask;
    }
}