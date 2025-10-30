namespace IQ.Mofy.Core.Fundamentals.Disposable;

public class Disposable : IDisposable
{
    private bool _disposed;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
            ReleaseManagedResources();
        ReleaseUnmanagedResources();
        _disposed = true;
    }

    #region ReleaseResources

    protected virtual void ReleaseManagedResources() { }

    protected virtual void ReleaseUnmanagedResources() { }

    #endregion

    ~Disposable() => Dispose(false);
}