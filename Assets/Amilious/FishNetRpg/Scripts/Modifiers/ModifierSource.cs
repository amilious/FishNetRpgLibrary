using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    
    /// <summary>
    /// This is a wrapper class for <see cref="IModifier"/>s that contains the source of the
    /// modifier and the time that the modifier was applied.
    /// </summary>
    public class ModifierSource<T> : IModifierSource where T : IModifier {
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the modifier.
        /// </summary>
        public T Modifier { get; }
        
        /// <inheritdoc />
        public int SourceId { get; }
        
        /// <inheritdoc />
        public float AppliedTime { get; }
        
        /// <inheritdoc />
        public float ExpireTime { get; }
        
        /// <inheritdoc />
        public bool DurationModifier { get; }

        /// <inheritdoc />
        public Systems System { get; }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Constructors ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constructor is used to create a new instance using the source object.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="source">The source of the modifier.</param>
        public ModifierSource(T modifier, Object source) {
            Modifier = modifier;
            SourceId = source.GetInstanceID();
            AppliedTime = Time.realtimeSinceStartup;
            DurationModifier = modifier.Duration > -1;
            ExpireTime = DurationModifier? Time.time + modifier.Duration : -1;
            System = modifier.System;
        }

        /// <summary>
        /// This constructor is used to create a new instance using the source's id.
        /// </summary>
        /// <param name="modifier">The modifier.</param>
        /// <param name="sourceId">The source id.</param>
        public ModifierSource(T modifier, int sourceId) {
            Modifier = modifier;
            SourceId = sourceId;
            AppliedTime = Time.time;
            DurationModifier = modifier.Duration > -1;
            ExpireTime = DurationModifier? Time.time + modifier.Duration : -1;
            System = modifier.System;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool HasSource(Object source) => SourceId == source.GetInstanceID();

        /// <inheritdoc />
        public bool HasSource(int sourceId) => SourceId == sourceId;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}