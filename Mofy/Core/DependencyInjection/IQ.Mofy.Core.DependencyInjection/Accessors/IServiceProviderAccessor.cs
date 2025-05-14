namespace IQ.Mofy.Core.DependencyInjection.Accessors;

public interface IServiceProviderAccessor : IAccessor
{
    public IServiceProvider ServiceProvider { get; }
}