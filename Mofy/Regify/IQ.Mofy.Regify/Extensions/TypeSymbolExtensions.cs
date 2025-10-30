using Microsoft.CodeAnalysis;

namespace IQ.Mofy.Regify.Extensions;

public static class TypeSymbolExtensions
{
    public static bool IsExplicitImplementationOf(this ITypeSymbol? self, string typeName)
    {
        if (self == null) return false;

        var baseInterfaces = self.Interfaces.SelectMany(i => i.Interfaces);

        return self.Interfaces.Except(baseInterfaces).Any(i => i.ToDisplayString() == typeName);
    }

    public static bool HasEmptyConstructor(this ITypeSymbol? self) => self is INamedTypeSymbol namedTypeSymbol && namedTypeSymbol.Constructors.Any(c => !c.TypeArguments.Any());
}