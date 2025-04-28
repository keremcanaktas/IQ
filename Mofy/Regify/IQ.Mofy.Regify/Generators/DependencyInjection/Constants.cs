namespace IQ.Mofy.Regify.Generators.DependencyInjection;

public static class Constants
{
    internal const string ServiceCollectionItemName = "IQ.Mofy.Core.Abstractions.DependencyInjection.Core.IServiceCollectionItem";
    internal const string IgnoredServiceName = "IQ.Mofy.Core.Abstractions.DependencyInjection.Core.IIgnoredService";
    internal const string RequiredServiceName = "IQ.Mofy.Core.Abstractions.DependencyInjection.IRequiredService";
    internal const string SingletonInstanceName = "IQ.Mofy.Core.Abstractions.DependencyInjection.Core.ISingletonInstance";

    internal const string ServiceTypesAttributeName = "IQ.Mofy.Core.Data.Annotations.DependencyInjection.ServiceTypesAttribute";
    internal const string LifeStyleAttributeName = "IQ.Mofy.Core.Data.Annotations.DependencyInjection.LifeStyleAttribute";

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