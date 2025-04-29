using Microsoft.CodeAnalysis;
// ReSharper disable MemberCanBePrivate.Global

namespace IQ.Mofy.Regify.Generators;

public abstract class Generator : IIncrementalGenerator
{
    public virtual void Initialize(IncrementalGeneratorInitializationContext context) { }
}