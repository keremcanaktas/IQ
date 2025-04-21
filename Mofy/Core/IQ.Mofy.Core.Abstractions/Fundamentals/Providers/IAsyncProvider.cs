namespace IQ.Mofy.Core.Abstractions.Fundamentals.Providers;

public interface IAsyncProvider : IProvider;

public interface IAsyncProvider<TResult> : IAsyncProvider
{
    Task<TResult> ProvideAsync();
}

public interface IAsyncProvider<in T, TResult> : IAsyncProvider
{
    Task<TResult> ProvideAsync(T argument);
}