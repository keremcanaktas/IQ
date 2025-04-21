using Microsoft.CodeAnalysis;

namespace IQ.Mofy.Regify.Generators.DependencyInjection.Data;

internal class ServiceTypesAttribute
{
    public List<ITypeSymbol> Types { get; set; } = [];
    public ServiceSelectorType ServiceSelectorType { get; set; }
    public TypedConstant? Key { get; set; }
}