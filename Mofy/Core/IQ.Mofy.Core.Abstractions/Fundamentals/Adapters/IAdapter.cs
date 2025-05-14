using IQ.Mofy.Core.DependencyInjection.Descriptors;

namespace IQ.Mofy.Core.Abstractions.Fundamentals.Adapters;

public interface IAdapter : ITransient;

public interface IAdapter<in T, out TResult> : IAdapter
{
    TResult Adapt(T value);
}