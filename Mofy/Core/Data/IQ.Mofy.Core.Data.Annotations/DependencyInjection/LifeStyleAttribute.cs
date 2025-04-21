// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
namespace IQ.Mofy.Core.Data.Annotations.DependencyInjection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public class LifeStyleAttribute(LifeStyle lifestyle) : Attribute
{
    public LifeStyle LifeStyle { get; set; } = lifestyle;

    public int ServiceLifeTime => ToServiceLifeTime(LifeStyle);

    public virtual int ToServiceLifeTime(LifeStyle lifestyle) => lifestyle switch
    {
        LifeStyle.Singleton => 0,
        LifeStyle.Scoped => 1,
        _ => 2,
    };
}