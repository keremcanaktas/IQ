namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasReadonlyServiceProvider
{
    public IServiceProvider ServiceProvider { get; }
}