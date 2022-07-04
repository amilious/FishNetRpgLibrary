using UnityEngine;

namespace Amilious.FishNetRpg.Statistics {
    
    [CreateAssetMenu(fileName = "NewStatModifier", menuName = FishNetRpg.ASSET_MENU_ROOT + "Stats/Stat Modifier", order=1)]
    public class StatModifer : ScriptableObject, IStatModifier {
        
        #region Inspector Variables ////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private string statName;
        [SerializeField, Tooltip("The type of the modifier.")] 
        private ModifierType modifierType;
        [SerializeField, Tooltip("The modifier amount.")] 
        private float amount;
        [SerializeField, Tooltip("If greater than -1, the time that the modifier will last.")] 
        private float duration = -1;

        #endregion
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        public string StatName => statName;
        
        /// <inheritdoc/>
        public float Amount => amount;
        
        /// <inheritdoc/>
        public ModifierType ModifierType => modifierType;
        
        /// <inheritdoc/>
        public float Duration => duration;

        #endregion
        
    }
    
}