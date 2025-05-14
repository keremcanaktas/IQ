using IQ.Mofy.Core.DependencyInjection.Descriptors;

namespace IQ.Mofy.Core.Abstractions.Fundamentals.Providers;

public interface IProvider : ITransient;

public interface IProvider<out TResult> : IProvider
{
    public TResult Provide();
}

public interface IProvider<in T, out TResult> : IProvider
{
    public TResult Provide(T argument);
}