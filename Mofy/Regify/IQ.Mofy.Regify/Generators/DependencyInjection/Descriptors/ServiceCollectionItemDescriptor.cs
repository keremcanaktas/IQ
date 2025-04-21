using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace IQ.Mofy.Regify.Generators.DependencyInjection.Descriptors;

public sealed class ServiceCollectionItemDescriptor
{
    public ITypeSymbol TypeSymbol { get; set; } = null!;
    
    public string ServiceTypeName { get; set; } = null!;
    public string ImplementationTypeName { get; set; } = null!;
    public string LifeStyle { get; set; } = "Transient";
    public object? Key { get; set; }


    private string? _internalKey;
    private string InternalKey => _internalKey ??= Key switch
    {
        string stringKey => $"\"{stringKey}\"",
        TypedConstant typedConstant => GetTypedConstantString(typedConstant),
        _ => Key?.ToString() ?? string.Empty
    };

    private string HasKey => !string.IsNullOrWhiteSpace(InternalKey) ? "Keyed" : string.Empty;

    private bool HasSingletonInstance => TypeSymbol.AllInterfaces.Any(t => t.ToDisplayString() == Constants.HasSingletonInstanceName);

    private string GenericArguments => $"{ServiceTypeName}{(ServiceTypeName != ImplementationTypeName ? $", {ImplementationTypeName}" : string.Empty)}";

    public override string ToString()
    {
        return HasSingletonInstance 
            ? $"services.Add{HasKey}Singleton<{ServiceTypeName}>(new {ImplementationTypeName}());" 
            : $"services.Add{HasKey}{LifeStyle}<{GenericArguments}>({InternalKey});";
    }

    private static string GetTypedConstantString(TypedConstant typedConstant)
    {
        if (typedConstant.Kind == TypedConstantKind.Error || typedConstant.IsNull) return string.Empty;
        
        var typedConstantString = typedConstant.ToCSharpString().Trim('{').Trim('}');

        if (string.IsNullOrWhiteSpace(typedConstantString)) return string.Empty;
        
        return typedConstant.Kind switch
        {
            TypedConstantKind.Array => $"new {typedConstant.Type} {{{typedConstantString}}}",
            _ => typedConstantString
        };
    }
}