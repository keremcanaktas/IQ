// See https://aka.ms/new-console-template for more information

using IQ.Mofy.Core.App;
using IQ.Mofy.Core.DependencyInjection.Decorators;
using IQ.Mofy.Core.DependencyInjection.Descriptors;
using IQ.Mofy.Core.Extensions;
using IQ.Mofy.Data.Abstractions.Entities;
using IQ.Test;
using Microsoft.Extensions.DependencyInjection;

var application = new Application();

application.ServiceCollection.AddRegify();

application.ServiceCollection.AddScoped(typeof(IRepo<>), typeof(Repo<>));


await application.RunAsync();


var serviceScope = application.ServiceProvider.CreateScope();

var queryableRepository = serviceScope.ServiceProvider.GetService<IProduct>();

Console.ReadLine();


public interface IRepo<T> where T : IEntity;

public class Repo<T> : IRepo<T> where T : IEntity
{

}

public class RepoWrapper<T>: IRepo<T>
    where T : IEntity
{
    
}

public interface IProduct : IScoped
{
    public string? Name { get; set; }
}

public class Product : IProduct
{
    public string? Name { get; set; }
}

public class ProductDecorator : ServiceDecorator<IProduct>
{
    public override IProduct Decorate(IServiceProvider serviceProvider, IProduct instance)
    {
        instance.Name = "Kerem";
        return instance;
    }
}