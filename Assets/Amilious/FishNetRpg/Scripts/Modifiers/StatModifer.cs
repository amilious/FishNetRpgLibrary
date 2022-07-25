using Amilious.FishNetRpg.Statistics;
using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    /// <summary>
    /// This class is used to modify values in the stat's system.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatModifier", menuName = FishNetRpg.MODIFIERS_MENU_ROOT + "Stat")]
    public class StatModifer : Modifier, IStatModifier {
        
        #region Inspector Variables ////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the stat that the modifier will be applied to.")] 
        private Stat stat;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Properties //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc/>
        public string StatName => stat.StatName;

        /// <inheritdoc/>
        public Stat Stat => stat;
        
        /// <inheritdoc/>
        public override Systems System => Systems.StatsSystem;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
    
}