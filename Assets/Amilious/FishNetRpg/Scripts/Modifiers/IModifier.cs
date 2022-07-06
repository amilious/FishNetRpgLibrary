namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This interface is the base of all modifiers.
    /// </summary>
    public interface IModifier {
        
        /// <summary>
        /// This property contains the system that the modifier is for.
        /// </summary>
        public Systems System { get; }

        /// <summary>
        /// This property contains the modifier's value.
        /// </summary>
        public float Amount { get; }
        
        /// <summary>
        /// This property contains the modifier's operation.
        /// </summary>
        public ModifierType ModifierType { get; }
        
        /// <summary>
        /// This property is used to add a duration for the modifier.  If this value is less than zero the modifier
        /// will not be automatically removed after a period of time.
        /// </summary>
        public float Duration { get; }
        
    }
    
}