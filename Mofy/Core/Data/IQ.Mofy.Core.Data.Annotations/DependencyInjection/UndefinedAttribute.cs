namespace IQ.Mofy.Core.Data.Annotations.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class UndefinedAttribute() : LifeStyleAttribute(LifeStyle.Undefined);