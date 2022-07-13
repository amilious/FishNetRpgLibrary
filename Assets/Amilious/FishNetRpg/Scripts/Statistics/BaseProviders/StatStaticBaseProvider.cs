using UnityEngine;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    [CreateAssetMenu(fileName = "NewStaticBaseProvider",
        menuName = FishNetRpg.STATS_MENU_ROOT + "Static Base Value Provider", order = 44)]
    public class StatStaticBaseProvider : StatBaseValueProvider {
        
        [SerializeField] private int cap;
        [SerializeField] private int minimum;
        [SerializeField] private int baseValue = 100;

        /// <inheritdoc />
        public override int GetMinimum(int level) => minimum;

        /// <inheritdoc />
        public override int GetCap(int level) => cap;

        /// <inheritdoc />
        public override int BaseValue(int level) => baseValue;
        
    }
}