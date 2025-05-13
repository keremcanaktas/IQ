// See https://aka.ms/new-console-template for more information

using IQ.Mofy.Core.App;
using IQ.Mofy.Data.Abstractions.Entities;
using IQ.Test;
using Microsoft.Extensions.DependencyInjection;

var application = new Application();

application.ServiceCollection.AddRegify();

IQ.Mofy.Core.Extensions.DependencyInjection.ServiceCollectionExtensions.AddScoped(application.ServiceCollection, typeof(IRepo<>), typeof(RepoWrapper<>));


await application.RunAsync();


var serviceScope = application.ServiceProvider.CreateScope();

var queryableRepository = serviceScope.ServiceProvider.GetService<IRepo<Car>>();

Console.ReadLine();


public interface IRepo<T> where T : IEntity;

public class Repo<T> : IRepo<T> where T : IEntity
{

}

public class RepoWrapper<T>: IRepo<T>
    where T : IEntity
{

}


