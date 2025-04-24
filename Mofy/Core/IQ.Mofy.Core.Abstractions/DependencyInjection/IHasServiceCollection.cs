using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasServiceCollection : IHasReadonlyServiceCollection
{
    public new IServiceCollection ServiceCollection { get; set; }
}