using Amilious.Core;
using UnityEngine;

namespace Amilious.FishNetRpg.Statistics {
    
    [CreateAssetMenu(fileName = "NewStatModifier", menuName = FishNetRpg.STATS_MENU_ROOT + "Stat Modifier", order=0)]
    public class StatModifer : Modifier, IStatModifier {
        
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

        public override ModifierSystems System => ModifierSystems.StatsSystem;
    }
    
}