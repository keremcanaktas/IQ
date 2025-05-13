using Microsoft.Extensions.DependencyInjection;

namespace IQ.Mofy.Core.DependencyInjection;

public class DefaultServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
{
    private readonly ServiceProviderOptions _options;

    public DefaultServiceProviderFactory() : this(new ServiceProviderOptions { ValidateOnBuild = false, ValidateScopes = false }) { }

    public DefaultServiceProviderFactory(ServiceProviderOptions options) => _options = options ?? throw new ArgumentNullException(nameof(options));

    public IServiceCollection CreateBuilder(IServiceCollection services) => services;

    public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder) => new ServiceProvider(containerBuilder.BuildServiceProvider(_options));
}