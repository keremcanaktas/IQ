namespace IQ.Mofy.Core.Abstractions.Fundamentals.Builders;

public interface IAsyncBuilder : IBuilder
{
    public Task BuildAsync();
}

public interface IAsyncBuilder<T> : IAsyncBuilder
{
    public new Task<T> BuildAsync();
}