using IQ.Mofy.Core.Abstractions.App;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.Configuration;

public static class ApplicationExtensions
{
    public static IApplication AddConfiguration(this IApplication self)
    {
        self.ServiceCollection.AddConfiguration();
        return self;
    }

    public static IConfiguration? GetConfiguration(this IApplication self) => self.ServiceProvider?.GetService<IConfiguration>() ?? self.ServiceCollection.GetConfiguration();

    public static IApplication Configure<TOptions>(this IApplication self) where TOptions : class
    {
        self.ServiceCollection.Configure<TOptions>();
        return self;
    }

    public static IApplication Configure<TOptions>(this IApplication self, string name) where TOptions : class
    {
        self.ServiceCollection.Configure<TOptions>(name);
        return self;
    }

    public static IApplication Configure<TOptions>(this IApplication self, IConfiguration configuration) where TOptions : class
    {
        self.ServiceCollection.Configure<TOptions>(configuration);
        return self;
    }

    public static IApplication Configure<TOptions>(this IApplication self, Action<TOptions> configureOptions) where TOptions : class
    {
        self.ServiceCollection.Configure(configureOptions);
        return self;
    }

    public static IApplication Configure<TOptions>(this IApplication self, IConfiguration configuration, Action<BinderOptions>? configureBinder) where TOptions : class
    {
        self.ServiceCollection.Configure<TOptions>(configuration, configureBinder);
        return self;
    }
}