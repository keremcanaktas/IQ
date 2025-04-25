// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.App;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;

var application = new WebApiApplication();

application.ServiceCollection.AddRegify();

await application.BuildHostAsync();

application.MapGet("/", async (ICarRepository carRepository) =>
{
    var query = await carRepository.GetQueryableAsync();

    var cars = await carRepository.GetListAsync(d => true);
    return cars;
});

await application.RunAsync();

Console.WriteLine("app");

Console.ReadLine();