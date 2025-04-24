using IQ.Mofy.Regify.Extensions;
using IQ.Mofy.Regify.Generators.DependencyInjection.Data;
using IQ.Mofy.Regify.Generators.DependencyInjection.Descriptors;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
// ReSharper disable ConvertIfStatementToReturnStatement

namespace IQ.Mofy.Regify.Generators.DependencyInjection;


[Generator]
public class ServiceCollectionItemsGenerator : Generator, IIncrementalGenerator
{
    public INamedTypeSymbol? ServiceTypeRequiredSymbol { get; set; }


    public override void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var classDeclarations = context.SyntaxProvider.CreateSyntaxProvider(predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax, transform: static (context, _) => context.Node as ClassDeclarationSyntax).Where(static m => m is not null);

        context.RegisterSourceOutput(context.CompilationProvider.Combine(classDeclarations.Collect()), (spc, source) =>
        {
            var registrations = string.Join("\n\t\t", GetServiceCollectionItemsDescriptors(source.Left));
            var sourceCode = Constants.ServiceCollectionExtensionsSourceCode.Replace("{Registrations}", registrations);

            spc.AddSource("ServiceCollectionExtensions.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
        });
    }

    #region ServiceColletionItems

    protected virtual IEnumerable<ServiceCollectionItemDescriptor> GetServiceCollectionItemsDescriptors(Compilation compilation) => compilation
        .GetAllTypeSymbols()
        .Where(FilterServiceCollectionItem)
        .SelectMany(GetServiceCollectionItemsDescriptors)
        .ToList();

    protected virtual bool FilterServiceCollectionItem(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol == null) return false;

        if (typeSymbol.IsAbstract) return false;
        if (typeSymbol is INamedTypeSymbol { IsGenericType: true }) return false;

        return typeSymbol.AllInterfaces.Any(i => i.ToDisplayString() == Constants.ServiceCollectionItemName);
    }

    protected virtual IEnumerable<ServiceCollectionItemDescriptor> GetServiceCollectionItemsDescriptors(ITypeSymbol typeSymbol)
    {
        return GetServiceTypesAttributes(typeSymbol)
            .SelectMany(
                serviceTypesAttribute => GetServiceTypeSymbols(typeSymbol, serviceTypesAttribute.Types, serviceTypesAttribute.ServiceSelectorType).Distinct(SymbolEqualityComparer.Default),
                (serviceTypesAttribute, serviceTypeSymbol) => new ServiceCollectionItemDescriptor
                {
                    TypeSymbol = typeSymbol,
                    ServiceTypeName = serviceTypeSymbol!.ToDisplayString(),
                    ImplementationTypeName = typeSymbol.ToDisplayString(),
                    Key = serviceTypesAttribute.Key,
                    LifeStyle = GetAllLifeStyle(typeSymbol)
                });
    }

    internal virtual IEnumerable<ServiceTypesAttribute> GetServiceTypesAttributes(ITypeSymbol typeSymbol)
    {
        var serviceTypesAttributes = typeSymbol
            .GetAttributes(Constants.ServiceTypesAttributeName)
            .Select(attributeData => new ServiceTypesAttribute
            {
                Types = attributeData.GetAttributeValue(nameof(ServiceTypesAttribute.Types), c => c.LastOrDefault())?.Values.Select(v => (ITypeSymbol)v.Value!).Concat(attributeData.GetAttributeGenericArguments()).ToList() ?? [],
                ServiceSelectorType = attributeData.GetAttributeValue(nameof(ServiceTypesAttribute.ServiceSelectorType))?.GetTypedConstantValue<ServiceSelectorType?>() ?? ServiceSelectorType.DefaultInterface,
                Key = attributeData.GetAttributeValue(nameof(ServiceTypesAttribute.Key), c => c.FirstOrDefault())
            }).ToList();

        if (serviceTypesAttributes.Count == 0)
            serviceTypesAttributes =
            [
                new ServiceTypesAttribute
                {
                    ServiceSelectorType = ServiceSelectorType.DefaultInterface,
                    Key = null
                }
            ];

        return serviceTypesAttributes;
    }

    private static IEnumerable<ITypeSymbol> GetServiceTypeSymbols(ITypeSymbol typeSymbol, List<ITypeSymbol> serviceTypes, ServiceSelectorType serviceSelectorType)
    {
        foreach (var type in serviceTypes)
            yield return type;

        var serviceTypeRequires = typeSymbol.AllInterfaces.Where(i => i.AllInterfaces.Any(ii => ii.ToDisplayString() == Constants.ServiceTypeRequiredName)).ToList();
        var inServiceTypeRequires = serviceTypeRequires.Where(i => !i.AllInterfaces.Any(ix => serviceTypeRequires.Contains(ix, SymbolEqualityComparer.Default))).ToList();

        foreach (var inServiceTypeRequire in inServiceTypeRequires)
            yield return inServiceTypeRequire;


        if (serviceTypes.Count != 0)
            serviceSelectorType = ServiceSelectorType.None;

        if (serviceSelectorType.HasFlag(ServiceSelectorType.Self))
            yield return typeSymbol;

        if (serviceSelectorType.HasFlag(ServiceSelectorType.AllInterface))
            foreach (var @interface in typeSymbol.AllInterfaces)
                yield return @interface;

        else if (serviceSelectorType.HasFlag(ServiceSelectorType.DefaultInterface))
        {
            var defaultInterface = typeSymbol.AllInterfaces.FirstOrDefault(i => i.Name == $"I{typeSymbol.Name}") ?? typeSymbol.AllInterfaces.FirstOrDefault();
            if (defaultInterface != null)
                yield return defaultInterface;
        }

        if (!serviceSelectorType.HasFlag(ServiceSelectorType.All)) yield break;

        if (typeSymbol.BaseType != null)
            yield return typeSymbol.BaseType;
    }

    private static string GetAllLifeStyle(ITypeSymbol? typeSymbol)
    {
        var lifeStyle = GetLifeStyle(typeSymbol) ?? GetLifeStyle(typeSymbol?.BaseType) ?? typeSymbol?.AllInterfaces.Select(GetLifeStyle).FirstOrDefault(l => l is not null);

        return lifeStyle ?? "Transient";
    }

    private static string? GetLifeStyle(ITypeSymbol? typeSymbol)
    {
        if (typeSymbol is null) return null;

        var lifestyleAttribute = typeSymbol.GetAttribute(Constants.LifeStyleAttributeName);

        var lifeStyle = lifestyleAttribute?.GetAttributeValue("LifeStyle", t => t.LastOrDefault())?.GetTypedConstantValue<LifeStyle?>()?.ToString() ?? (lifestyleAttribute?.AttributeClass?.Name).TrimEnd("Attribute");

        return lifeStyle;
    }

    #endregion
}