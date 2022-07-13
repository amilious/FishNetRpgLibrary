using Amilious.FishNetRpg.Modifiers;
namespace Amilious.FishNetRpg.Statistics {
    
    /// <summary>
    /// This interface is used to modify stats.
    /// </summary>
    public interface IStatModifier : IModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the stat that the modifier should be applied to.
        /// </summary>
        public string StatName { get; }
        
        /// <summary>
        /// This property contains the <see cref="Stat"/> that the modifier is for.
        /// </summary>
        public Stat Stat { get; }
        
        #endregion
        
    }

}