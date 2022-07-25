namespace Amilious.Core.Threading {
    
    /// <summary>
    /// Describes the state of a future.
    /// </summary>
    public enum FutureState {
        
        /// <summary>
        /// The future hasn't begun to resolve a value.
        /// </summary>
        Pending,

        /// <summary>
        /// The future is working on resolving a value.
        /// </summary>
        Processing,

        /// <summary>
        /// The future has a value ready.
        /// </summary>
        Success,

        /// <summary>
        /// The future failed to resolve a value.
        /// </summary>
        Error,
        
        /// <summary>
        /// The future was canceled.
        /// </summary>
        Canceled
    }
    
}