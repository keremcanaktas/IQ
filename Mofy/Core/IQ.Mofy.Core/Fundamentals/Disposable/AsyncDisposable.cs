namespace IQ.Mofy.Core.Fundamentals.Disposable;

public class AsyncDisposable : Disposable, IAsyncDisposable
{
    #region IAsyncDisposable

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await ReleaseManagedResourcesAsync();
        await ReleaseUnmanagedResourcesAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(disposing: false);
        GC.SuppressFinalize(this);
    }

    #endregion
    
    #region ReleaseResources

    protected virtual ValueTask ReleaseManagedResourcesAsync() => ValueTask.CompletedTask;

    protected virtual ValueTask ReleaseUnmanagedResourcesAsync() => ValueTask.CompletedTask;

    #endregion
}