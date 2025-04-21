using IQ.Mofy.Core.Abstractions.Fundamentals.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.Abstractions.DependencyInjection;

public interface IServiceProviderFactory : IServiceProviderFactory<object>, IFactory;