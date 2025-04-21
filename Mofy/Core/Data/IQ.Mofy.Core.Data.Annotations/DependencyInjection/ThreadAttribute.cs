namespace IQ.Mofy.Core.Data.Annotations.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class ThreadAttribute() : LifeStyleAttribute(LifeStyle.Thread);