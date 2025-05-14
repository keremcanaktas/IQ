namespace IQ.Mofy.Core.DependencyInjection.Accessors;

public interface IObjectAccessor<T> : IAccessor
{
    T Value { get; }
}