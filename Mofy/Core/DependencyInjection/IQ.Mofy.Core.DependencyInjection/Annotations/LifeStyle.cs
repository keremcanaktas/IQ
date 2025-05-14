namespace IQ.Mofy.Core.DependencyInjection.Annotations;

public enum LifeStyle
{
    Undefined = 0,
    Singleton = 1,
    Thread = 2,
    Transient = 3,
    Pooled = 4,
    Custom = 6,
    Scoped = 7,
    Bound = 8,
}