using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Statistics.BaseProviders;
using UnityEngine;

namespace Amilious.FishNetRpg.Statistics {
    
    [CreateAssetMenu(fileName = "NewStat", menuName = FishNetRpg.ASSET_MENU_ROOT+"Stats/Stat",order = 0)]
    public class Stat : ScriptableObject {

        [SerializeField] private string statName;
        [SerializeField] private StatBaseValueProvider baseValueProvider;

        /// <summary>
        /// This property contains the stat's name.
        /// </summary>
        public string StatName => statName;

        /// <summary>
        /// This property contains the stat's base value provider.
        /// </summary>
        public StatBaseValueProvider BaseValueProvider => baseValueProvider;

        public StatController this[Entity entity] => entity.StatManager[this];

        public bool TryGetValue(Entity entity, out int value) {
            value = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            value = statInfo.Value;
            return true;
        }
        
        public bool TryGetLevel(Entity entity, out int value) {
            value = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            value = statInfo.Level;
            return true;
        }
        
        public bool TryGetBaseValue(Entity entity, out int value) {
            value = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            value = statInfo.BaseValue;
            return true;
        }

        private bool TryGetStatInfo(Entity entity, out StatController statController) {
            statController = default;
            if(entity == null) return false;
            statController = entity.StatManager[this];
            return statController is { Initialized: true };
        }

    }

}
