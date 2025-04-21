using Microsoft.CodeAnalysis;

namespace IQ.Mofy.Regify.Visitors;

public abstract class SymbolVisitor
{
    public virtual void Visit(ISymbol symbol)
    {
        switch (symbol)
        {
            case INamespaceSymbol namespaceSymbol: VisitNamespaceSymbol(namespaceSymbol); break;
            case ITypeSymbol typeSymbol: VisitTypeSymbol(typeSymbol); break;
        }
    }

    protected virtual void VisitNamespaceSymbol(INamespaceSymbol namespaceSymbol) { }
    
    protected virtual void VisitTypeSymbol(ITypeSymbol typeSymbol) { }
}