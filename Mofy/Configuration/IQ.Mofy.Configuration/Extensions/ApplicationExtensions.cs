using IQ.Mofy.Core.Abstractions.App;
using IQ.Mofy.Core.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IQ.Mofy.Configuration.Extensions;

public static class ApplicationExtensions
{
    public static IApplication AddConfiguration(this IApplication self)
    {
        self.ServiceCollection.AddConfiguration();
        return self;
    }

    public static IConfiguration? GetConfiguration(this IApplication self) => self.ServiceProvider.GetService<IConfiguration>() ?? self.ServiceCollection.GetService<IConfiguration>();
}