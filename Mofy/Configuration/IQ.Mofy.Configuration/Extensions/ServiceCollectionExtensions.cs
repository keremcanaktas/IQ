using IQ.Mofy.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IQ.Mofy.Configuration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection self)
    {
        var configurationBuilder = self.GetService<IConfigurationBuilder>() ?? new ConfigurationBuilder();
        self.TryAddSingleton<IConfiguration>(configurationBuilder.Build());
        return self;
    }
}