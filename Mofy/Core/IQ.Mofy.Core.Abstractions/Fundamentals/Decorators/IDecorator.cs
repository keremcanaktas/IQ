namespace IQ.Mofy.Core.Abstractions.Fundamentals.Decorators;

public interface IDecorator;

public interface IDecorator<TInstance> : IDecorator
{
    void Decorate(TInstance instance);
}

public interface IDecorator<TInstance, TArguments> : IDecorator
{
    void Decorate(TInstance instance, TArguments arguments);
}