namespace IQ.Mofy.Core.DependencyInjection.Annotations;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class ThreadAttribute() : LifeStyleAttribute(LifeStyle.Thread);