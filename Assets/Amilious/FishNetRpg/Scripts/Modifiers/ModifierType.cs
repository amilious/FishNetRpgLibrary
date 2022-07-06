namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This class is used to represent a modifier type.
    /// </summary>
    public enum ModifierType {
        
        /// <summary>
        /// This value indicates that a value should be added to the base value before the multipliers.
        /// </summary>
        Additive = 0,
        
        /// <summary>
        /// All modifiers of this type will have their values added together before multiplying the stats base
        /// value after applying the additive modifiers.
        /// </summary>
        AdditiveMultiplier = 5,
        
        /// <summary>
        /// Modifiers of this type will multiply the base value individually after applying the AdditiveMultiplier
        /// modifiers and the additive multipliers. 
        /// </summary>
        StackableMultiplier = 10,
        
        /// <summary>
        /// Modifiers of this type will be added to the base value after the multiplier modifiers have been applied.
        /// </summary>
        PostMultiplierAdditive = 15,
        
        /// <summary>
        /// Modifiers of this type will override the base value.
        /// </summary>
        Override = 20
        
    }
}