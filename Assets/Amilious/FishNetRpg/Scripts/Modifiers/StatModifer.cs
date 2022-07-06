using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This class is used to modify values in the stat's system.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatModifier", menuName = FishNetRpg.STATS_MENU_ROOT + "Modifier", order=0)]
    public class StatModifer : Modifier, IStatModifier {
        
        #region Inspector Variables ////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private string statName;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        public string StatName => statName;
        
        /// <inheritdoc/>
        public override Systems System => Systems.StatsSystem;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}