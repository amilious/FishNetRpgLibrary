namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This interface is used to get the base value based on the stats level.
    /// </summary>
    public interface IStatBaseValueProvider {

        /// <summary>
        /// This method is used to get the stats base value based on the given level.
        /// </summary>
        /// <param name="level">The stats level.</param>
        /// <returns>The base value for the given level.</returns>
        public int GetBaseValue(int level);
        
        /// <summary>
        /// This property contains the maximum value that a stat can have.  If this value is less
        /// than zero there will be no cap.
        /// </summary>
        public int Cap { get; }

    }
    
}
