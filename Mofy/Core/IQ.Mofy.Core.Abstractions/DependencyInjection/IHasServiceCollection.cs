using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IHasServiceCollection : IHasReadonlyServiceCollection, IHasServiceItem
{
    public new IServiceCollection ServiceCollection { get; set; }
}