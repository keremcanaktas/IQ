using IQ.Mofy.Core.Data.Annotations.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

[Transient]
public interface ITransient : IServiceCollectionItem;