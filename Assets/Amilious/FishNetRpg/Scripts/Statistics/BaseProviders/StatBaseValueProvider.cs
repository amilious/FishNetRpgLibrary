using UnityEngine;

namespace Amilious.FishNetRpg.Statistics.BaseProviders {
    
    public abstract class StatBaseValueProvider : ScriptableObject {

        public abstract int GetMinimum(int level);

        public abstract int GetCap(int level);

        public abstract int BaseValue(int level);

    }
}