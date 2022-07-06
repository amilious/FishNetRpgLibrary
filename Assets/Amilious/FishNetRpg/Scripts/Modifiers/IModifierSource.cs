namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This interface is used to get information about the <see cref="ModifierSource{T}"/> without needing the
    /// generic type.
    /// </summary>
    public interface IModifierSource {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the source id for the modifier's source.
        /// </summary>
        public int SourceId { get; }
        
        /// <summary>
        /// This property contains the time when the modifier was applied.
        /// </summary>
        public float AppliedTime { get; }
        
        /// <summary>
        /// If the modifier is a duration modifier this property will contains the time that the modifier will
        /// expire.
        /// </summary>
        public float ExpireTime { get; }
        
        /// <summary>
        /// This property is true if the modifier is a duration modifier, otherwise false.
        /// </summary>
        public bool DurationModifier { get; }
        
        /// <summary>
        /// This property contains system that the modifier effects.
        /// </summary>
        public Systems System { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the given source is the source of this modifier.
        /// </summary>
        /// <param name="source">The source that you want to compare.</param>
        /// <returns>True if the given source is the source of this modifier.</returns>
        public bool HasSource(UnityEngine.Object source);

        /// <summary>
        /// This method is used to check if the given source id is the source id of this modifer.
        /// </summary>
        /// <param name="sourceId">The source id of this modifier.</param>
        /// <returns>True if the given source id is the source id of this modifier.</returns>
        public bool HasSource(int sourceId);
     
        #endregion
        
    }
}