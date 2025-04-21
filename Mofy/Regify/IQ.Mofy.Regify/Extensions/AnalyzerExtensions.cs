using System.Collections.Immutable;
using IQ.Mofy.Regify.Visitors;
using Microsoft.CodeAnalysis;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ConvertIfStatementToReturnStatement

namespace IQ.Mofy.Regify.Extensions;

public static class AnalyzerExtensions
{
    public static IEnumerable<IAssemblySymbol> GetAllAssemblySymbols(this Compilation compilation)
    {
        return compilation
            .SourceModule
            .ReferencedAssemblySymbols
            .Concat([compilation.Assembly]);
    }

    public static IEnumerable<INamespaceOrTypeSymbol> GetAllAssemblyGlobalNameSpaceMembers(this Compilation compilation)
    {
        return  compilation
            .GetAllAssemblySymbols()
            .SelectMany(assemblySymbol => assemblySymbol.GlobalNamespace.GetMembers());
    }

    public static IEnumerable<ITypeSymbol> GetAllTypeSymbols(this Compilation compilation)
    {
        return compilation
            .GetAllAssemblyGlobalNameSpaceMembers()
            .SelectMany(GetTypeSymbols);
    }
    
    public static IEnumerable<ITypeSymbol> GetTypeSymbols(this INamespaceOrTypeSymbol namespaceOrTypeSymbol)
    {
        var visitor = new TypeSymbolFinderVisitor();
        
        visitor.Visit(namespaceOrTypeSymbol);

        return visitor.Types;
    }

    public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string name) => symbol.GetAttributes().Where(attributeData => FindAttribute(attributeData.AttributeClass, name));

    private static bool FindAttribute(this ITypeSymbol? typeSymbol, string name)
    {
        if(typeSymbol == null) return false;

        if (typeSymbol.ToDisplayString() == name) return true;

        return typeSymbol.BaseType is not null && FindAttribute(typeSymbol.BaseType, name);
    }

    public static AttributeData? GetAttribute(this ISymbol symbol, string name) => symbol.GetAttributes(name).FirstOrDefault();
    
    public static TypedConstant? GetAttributeValue(this AttributeData? attributeData, string key, Func<ImmutableArray<TypedConstant>, TypedConstant>? constructorArgumentSelector = null)
    {
        if (attributeData is null) return null;
        
        var typedConstant = attributeData.NamedArguments.FirstOrDefault(na => na.Key == key).Value;
        if (typedConstant.Kind != TypedConstantKind.Error) return typedConstant;

        return constructorArgumentSelector?.Invoke(attributeData.ConstructorArguments) ?? default;
    }
    
    public static TValue? GetTypedConstantValue<TValue>(this TypedConstant typedConstant) => TryCast<TValue>(typedConstant.Value, out var castedValue) ? castedValue : default;
    
    public static IEnumerable<TValue> GetTypedConstantValues<TValue>(this TypedConstant typedConstant)
    {
        if (typedConstant.Kind != TypedConstantKind.Array) return [];

        return typedConstant.Values.Select(t => t.GetTypedConstantValue<TValue>()!);
    }

    public static bool TryCast<T>(object? value, out T result)
    {
        try
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (type.IsEnum)
            {
                result = (T)Enum.Parse(type, value?.ToString() ?? string.Empty);
                return true;
            }
            
            switch (value)
            {
                case null:
                    result = default!;
                    return false;
                case T castedValue:
                    result = castedValue;
                    return true;
                default:
                    result = (T)Convert.ChangeType(value, typeof(T));
                    return true;
            }
        }
        catch
        {
            result = default!;
            return false;
        }
    }
}