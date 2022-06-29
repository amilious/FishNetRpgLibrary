
using FishNetRpgLibrary.Statistics;

namespace FishNetRpgLibrary {
    
    public interface IStatModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the name of the stat that the modifier should be applied to.
        /// </summary>
        public string StatName { get; }
        
        /// <summary>
        /// This property contains the modifier's value.
        /// </summary>
        public float Amount { get; }
        
        /// <summary>
        /// This property contains the modifier's operation.
        /// </summary>
        public ModifierOperation Operation { get; }
        
        /// <summary>
        /// This property is used to add a duration for the modifier.  If this value is less than zero the modifier
        /// will not be automatically removed after a period of time.
        /// </summary>
        public float Duration { get; }
        
        #endregion

    }
    
}