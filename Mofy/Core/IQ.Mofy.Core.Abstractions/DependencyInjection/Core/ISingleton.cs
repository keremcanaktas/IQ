using IQ.Mofy.Core.Data.Annotations.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

[Singleton]
public interface ISingleton : IServiceCollectionItem;