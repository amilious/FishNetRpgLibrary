using UnityEngine;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    /// <summary>
    /// This class is used to provide a table of stat base values based on level.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTableBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Table Base Value Provider", order = 45)]
    public class StatTableBaseProvider : StatBaseValueProvider {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private int cap;
        [SerializeField] private int minimum;
        [SerializeField] private int[] baseValues;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override int GetMinimum(int level) => minimum;

        /// <inheritdoc />
        public override int GetCap(int level) => cap;

        /// <inheritdoc />
        public override int BaseValue(int level) {
            if(level<=0) return 0;
            return level >= baseValues.Length ? baseValues[^1] : baseValues[level];
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}