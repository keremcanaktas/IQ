// See https://aka.ms/new-console-template for more information

using ConsoleWebApplication;
using IQ.Mofy.Core.App;
using IQ.Mofy.Core.App.Steps;
using IQ.Mofy.Web.Api.App;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var application = new WebApiApplication();
application.ServiceCollection.AddRegify();

await application.RunAsync();



public class ApplicationPostRunStep : ApplicationPostRunStep<WebApiApplication>
{
    public override Task OnPostRunAsync(WebApiApplication apiApplication)
    {
        var serviceScope = apiApplication.ServiceProvider.CreateScope();

        var carRepository = serviceScope.ServiceProvider.GetRequiredService<ICarRepository>();

        carRepository.DeleteRangeAsync([1]);

        apiApplication.MapGet("/", async (ICarRepository carRepository) => await carRepository.GetListAsync(d => true));



        return Task.CompletedTask;
    }
}


public class Interceptor : DispatchProxy
{
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        throw new NotImplementedException();
    }
}