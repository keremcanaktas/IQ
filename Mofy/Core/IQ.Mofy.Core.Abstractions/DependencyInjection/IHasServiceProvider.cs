namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasServiceProvider : IHasReadonlyServiceProvider, IHasServiceItem
{
    public new IServiceProvider ServiceProvider { get; set; }
}