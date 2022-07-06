using Amilious.Core;
using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This is the base class for scriptable object modifiers.
    /// </summary>
    public abstract class Modifier : AmiliousScriptableObject, IModifier {
        
        [SerializeField, Tooltip("The type of the modifier.")] 
        private ModifierType modifierType;
        [SerializeField, Tooltip("The modifier amount.")] 
        private float amount;
        [SerializeField, Tooltip("If greater than -1, the time that the modifier will last.")] 
        private float duration = -1;
        
        /// <inheritdoc />
        public abstract Systems System { get; }

        /// <inheritdoc />
        public virtual float Amount => amount;

        /// <inheritdoc />
        public virtual ModifierType ModifierType => modifierType;

        /// <inheritdoc />
        public virtual float Duration => duration;
    }
}