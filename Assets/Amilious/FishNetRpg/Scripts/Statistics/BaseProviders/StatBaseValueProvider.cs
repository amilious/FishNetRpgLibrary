using Amilious.Core;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    /// <summary>
    /// This is the base class for stat base value providers.
    /// </summary>
    public abstract class StatBaseValueProvider : AmiliousScriptableObject {

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the minimum value for the stat.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The minimum value for the stat.</returns>
        public abstract int GetMinimum(int level);

        /// <summary>
        /// This method is used to get the cap value for the stat.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The cap value used for the stat.</returns>
        public abstract int GetCap(int level);

        /// <summary>
        /// This method is used to get the base value of the stat for the given level.
        /// </summary>
        /// <param name="level">The stat's level.</param>
        /// <returns>The base value for the stat's level.</returns>
        public abstract int BaseValue(int level);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
}