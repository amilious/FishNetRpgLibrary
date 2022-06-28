using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This struct is used to modify a stat.
    /// </summary>
    public readonly struct StatModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains and id for the source of the modifier. 
        /// </summary>
        public int? SourceId { get; }
        
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


        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This constructor is used to create a new stat modifier.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        /// <param name="operation">The modifier operation.</param>
        /// <param name="amount">The modifier's amount.</param>
        /// <param name="duration">The duration of the modifier or -1 for no duration.</param>
        public StatModifier(Object source, ModifierOperation operation, float amount, float duration = -1f) {
            SourceId = source.GetInstanceID();
            Operation = operation;
            Amount = amount;
            Duration = duration;
        }
        
        /// <summary>
        /// This constructor is used to create a new stat modifier.
        /// </summary>
        /// <param name="sourceId">An integer representation of the source of the modifier.</param>
        /// <param name="operation">The modifier operation.</param>
        /// <param name="amount">The modifier's amount.</param>
        /// <param name="duration">The duration of the modifier or -1 for no duration.</param>
        public StatModifier(int sourceId, ModifierOperation operation, float amount, float duration = -1f) {
            SourceId = sourceId;
            Operation = operation;
            Amount = amount;
            Duration = duration;
        }

        /// <summary>
        /// This constructor is used to create a new stat modifier.
        /// </summary>
        /// <param name="operation">The modifier operation.</param>
        /// <param name="amount">The modifier's amount.</param>
        /// <param name="duration">The duration of the modifier or -1 for no duration.</param>
        public StatModifier(ModifierOperation operation, float amount, float duration = -1f) {
            SourceId = null;
            Operation = operation;
            Amount = amount;
            Duration = duration;
        }
        
        #endregion
        
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if this modifier has the given source.
        /// </summary>
        /// <param name="source">The source that you want to check for.</param>
        /// <returns>True if the modifier has the given source.</returns>
        public bool HasSource(Object source) => SourceId.HasValue && SourceId.Value == source.GetInstanceID();
        
        /// <summary>
        /// This method is used to check if this modifier has the given source.
        /// </summary>
        /// <param name="sourceId">The id of the source that you want to check for.</param>
        /// <returns>True if the modifier has the given source.</returns>
        public bool HasSource(int sourceId) => SourceId == sourceId;

        /// <summary>
        /// This method is used to apply a modifier to the given value.
        /// </summary>
        /// <param name="valueToModify">The value that you want to modify.</param>
        /// <returns>The modified value.</returns>
        public int ApplyModifier(int valueToModify) => Operation.ApplyModifier(valueToModify, Amount);

        #endregion

    }
    
}