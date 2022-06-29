using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This struct is used to modify a stat.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatModifier", 
        menuName = FishNetRpg.ASSET_MENU_ROOT+"Stats/Stat Modifier", order = 0)]
    public class StatModifier : ScriptableObject, IStatModifier {

        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private string statName;
        [SerializeField, Tooltip("The operation type of the modifier.")] 
        private ModifierOperation operation;
        [SerializeField, Tooltip("The modifier amount.")] 
        private float amount;
        [SerializeField, Tooltip("If greater than -1, the time that the modifier will last.")] 
        private float duration = -1;
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public string StatName { get => statName; }
        
        /// <inheritdoc />
        public float Amount { get => amount; }
        
        /// <inheritdoc />
        public ModifierOperation Operation { get => operation; }
        
        /// <inheritdoc />
        public float Duration { get => duration; }

        #endregion

    }
    
}