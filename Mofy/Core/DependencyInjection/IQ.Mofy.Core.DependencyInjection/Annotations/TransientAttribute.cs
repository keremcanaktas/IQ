namespace IQ.Mofy.Core.DependencyInjection.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class TransientAttribute() : LifeStyleAttribute(LifeStyle.Transient);