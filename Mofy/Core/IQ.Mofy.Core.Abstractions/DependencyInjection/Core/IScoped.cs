using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;
using IQ.Mofy.Core.Data.Annotations.DependencyInjection;

// ReSharper disable CheckNamespace

namespace IQ.Mofy.Core.Abstractions.DependencyInjection.Services;

[Scoped]
public interface IScoped : IServiceCollectionItem;