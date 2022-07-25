namespace Amilious.Core.Threading {
    
    /// <summary>
    /// This interface is used to be able to cancel any
    /// reusableFuture.
    /// </summary>
    public interface IReusableFuture {
        
        /// <summary>
        /// This method is used to cancel an existing process.
        /// </summary>
        /// <returns>True if the process was canceled, otherwise returns
        /// false if there was not a process to cancel.</returns>
        bool Cancel();
        
    }
    
}