using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This is a wrapper class for StatModifiers that contains the source of the
    /// modifier and the time that the modifier was applied.
    /// </summary>
    public class StatModifierSource {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the modifier.
        /// </summary>
        public IStatModifier Modifier { get; }
        
        /// <summary>
        /// This property contains the source id for the modifier's source.
        /// </summary>
        public int SourceId { get; }
        
        /// <summary>
        /// This property contains the time when the modifier was applied.
        /// </summary>
        public float AppliedTime { get; }

        #endregion
        
        
        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new instance using the source object.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="source">The source of the modifier.</param>
        public StatModifierSource(IStatModifier modifier, Object source) {
            Modifier = modifier;
            SourceId = source.GetInstanceID();
            AppliedTime = Time.realtimeSinceStartup;
        }

        /// <summary>
        /// This constructor is used to create a new instance using the source's id.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="sourceId">The source id.</param>
        public StatModifierSource(IStatModifier modifier, int sourceId) {
            Modifier = modifier;
            SourceId = sourceId;
            AppliedTime = Time.realtimeSinceStartup;
        }

        #endregion
        
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if the given source is the source of this modifier.
        /// </summary>
        /// <param name="source">The source that you want to compare.</param>
        /// <returns>True if the given source is the source of this modifier.</returns>
        public bool HasSource(Object source) => SourceId == source.GetInstanceID();

        /// <summary>
        /// This method is used to check if the given source id is the source id of this modifer.
        /// </summary>
        /// <param name="sourceId">The source id of this modifier.</param>
        /// <returns>True if the given source id is the source id of this modifier.</returns>
        public bool HasSource(int sourceId) => SourceId == sourceId;

        #endregion
        
    }
    
}