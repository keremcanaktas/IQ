using IQ.Mofy.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection self)
    {
        var configurationBuilder = self.GetService<IConfigurationBuilder>() ?? new ConfigurationBuilder();
        self.TryAddSingleton<IConfiguration>(configurationBuilder.Build());
        return self;
    }

    public static IConfiguration? GetConfiguration(this IServiceCollection self) => self.GetService<IConfiguration>();

    public static IServiceCollection Configure<TOptions>(this IServiceCollection self, string? prefix = null) where TOptions : class => self.Configure<TOptions>(typeof(TOptions).Name, prefix);

    public static IServiceCollection Configure<TOptions>(this IServiceCollection self, string key, string? prefix = null) where TOptions : class => self.Configure<TOptions>(c => c.GetSection($"{prefix}:{key}"));

    public static IServiceCollection Configure<TOptions>(this IServiceCollection self, Func<IConfiguration, IConfigurationSection> sectionSelector) where TOptions : class
    {
        var configuration = self.GetConfiguration();

        if (configuration is null) return self;

        self.Configure<TOptions>(sectionSelector(configuration));

        return self;
    }
}