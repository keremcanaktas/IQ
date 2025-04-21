using IQ.Mofy.Core.Abstractions.DependencyInjection.Core;

namespace IQ.Mofy.Core.Abstractions.Fundamentals.Builders;

public interface IBuilder : ITransient;

public interface IBuilder<out T> : IBuilder
{
    T Build();
}