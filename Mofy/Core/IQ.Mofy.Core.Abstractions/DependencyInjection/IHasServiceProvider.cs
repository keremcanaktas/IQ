namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasServiceProvider : IHasReadonlyServiceProvider
{
    public new IServiceProvider ServiceProvider { get; set; }
}