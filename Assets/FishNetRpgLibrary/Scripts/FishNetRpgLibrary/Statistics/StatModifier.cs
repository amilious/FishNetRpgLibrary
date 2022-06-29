using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This struct is used to modify a stat.
    /// </summary>
    public class StatModifier : ScriptableObject, IStatModifier {

        [SerializeField] private string stat;
        [SerializeField] private ModifierOperation operation;
        [SerializeField] private float amount;
        [SerializeField] private float duration = -1;
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public string StatName { get => stat; }
        
        /// <inheritdoc />
        public float Amount { get => amount; }
        
        /// <inheritdoc />
        public ModifierOperation Operation { get => operation; }
        
        /// <inheritdoc />
        public float Duration { get => duration; }

        #endregion

    }
    
}