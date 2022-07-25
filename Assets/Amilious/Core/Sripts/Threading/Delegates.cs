namespace Amilious.Core.Threading {
    
    /// <summary>
    /// Defines the signature for callbacks used by the future.
    /// </summary>
    /// <param name="future">The future.</param>
    public delegate void FutureCallback<T>(IFuture<T> future);
    
}