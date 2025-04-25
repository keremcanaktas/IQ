// See https://aka.ms/new-console-template for more information

using IQ.Mofy.Configuration.Extensions;
using IQ.Mofy.Core.App;
using IQ.Test;
using Microsoft.Extensions.DependencyInjection;

var application = new Application();

application.ServiceCollection.AddRegify();


await application.RunAsync();

var configuration = application.GetConfiguration();

var scope = application.ServiceProvider.CreateScope();

var carRepository = scope.ServiceProvider.GetRequiredService<ICarRepository>();

var car = await carRepository.GetListAsync(e => e.Id == 1);


scope.Dispose();


Console.Write(carRepository);



public class MyServiceProviderFactory 