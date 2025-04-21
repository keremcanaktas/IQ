using Microsoft.CodeAnalysis;

namespace IQ.Mofy.Regify.Visitors;

public class TypeSymbolFinderVisitor : SymbolVisitor
{
    public IList<ITypeSymbol> Types { get; } = new List<ITypeSymbol>();
    
    protected override void VisitNamespaceSymbol(INamespaceSymbol namespaceSymbol)
    {
        foreach (var member in namespaceSymbol.GetMembers())
            Visit(member);
    }

    protected override void VisitTypeSymbol(ITypeSymbol typeSymbol) => Types.Add(typeSymbol);
}