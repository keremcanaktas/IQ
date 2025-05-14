using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable MemberCanBePrivate.Global

namespace IQ.Mofy.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IApplication GetApplication(this IServiceCollection self) => self.GetRequiredService<IApplication>();
}