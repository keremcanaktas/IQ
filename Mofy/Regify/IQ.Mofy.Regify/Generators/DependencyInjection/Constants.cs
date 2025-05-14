namespace IQ.Mofy.Regify.Generators.DependencyInjection;

public static class Constants
{
    internal const string ServiceCollectionItemName = "IQ.Mofy.Core.DependencyInjection.Descriptors.IServiceDescriptor";
    internal const string IgnoredServiceName = "IQ.Mofy.Core.DependencyInjection.Services.IIgnoredService";
    internal const string RequiredServiceName = "IQ.Mofy.Core.DependencyInjection.Services.IRequiredService";
    internal const string SingletonInstanceName = "IQ.Mofy.Core.DependencyInjection.Descriptors.ISingletonInstance";

    internal const string ServiceTypesAttributeName = "IQ.Mofy.Core.DependencyInjection.Annotations.ServiceTypesAttribute";
    internal const string LifeStyleAttributeName = "IQ.Mofy.Core.DependencyInjection.Annotations.LifeStyleAttribute";

    public const string ServiceCollectionExtensionsSourceCode = """
                                                                using IQ.Mofy.Core.Extensions.DependencyInjection;

                                                                namespace IQ.Mofy.Core.App;

                                                                public static class ServiceCollectionExtensions
                                                                {
                                                                    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddRegify(this Microsoft.Extensions.DependencyInjection.IServiceCollection services)
                                                                    {
                                                                        {Registrations}
                                                                        return services;
                                                                    }
                                                                }
                                                                """;
}