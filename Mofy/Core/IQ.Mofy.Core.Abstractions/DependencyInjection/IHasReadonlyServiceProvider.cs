namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasReadonlyServiceProvider : IHasServiceCollectionItem
{
    public IServiceProvider ServiceProvider { get; }
}