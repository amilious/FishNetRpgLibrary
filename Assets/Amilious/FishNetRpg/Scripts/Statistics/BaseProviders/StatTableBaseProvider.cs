using UnityEngine;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    [CreateAssetMenu(fileName = "NewStatBaseProvider", 
        menuName = FishNetRpg.ASSET_MENU_ROOT + "Stats/Base Value Provider/Table", order=2)]
    public class StatTableBaseProvider : StatBaseValueProvider {

        
        [SerializeField] private int cap;
        [SerializeField] private int minimum;
        [SerializeField] private int[] baseValues;


        public override int GetMinimum(int level) => minimum;

        public override int GetCap(int level) => cap;

        public override int BaseValue(int level) {
            if(level<=0) return 0;
            return level >= baseValues.Length ? baseValues[^1] : baseValues[level];
        }
    }
}