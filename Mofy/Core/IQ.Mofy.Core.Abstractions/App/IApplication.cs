using IQ.Mofy.Core.DependencyInjection.Accessors;

namespace IQ.Mofy.Core.Abstractions.App;

public interface IApplication :
    IServiceCollectionAccessor,
    IServiceProviderAccessor,
    IAsyncDisposable,
    IDisposable
{
    IApplicationOptions Options { get; set; }
    
    public Task RunAsync();

    public Task StopAsync();
}

public interface IApplication<TOptions> : IApplication where TOptions : IApplicationOptions
{
    public new TOptions Options { get; set; }
}