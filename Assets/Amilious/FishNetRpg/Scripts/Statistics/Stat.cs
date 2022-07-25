using UnityEngine;
using Amilious.Core;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Statistics.BaseProviders;

namespace Amilious.FishNetRpg.Statistics {
    
    /// <summary>
    /// This class is used to represent a stat.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStat", menuName = FishNetRpg.STATS_MENU_ROOT+"Stat",order = 20)]
    public class Stat : AmiliousScriptableObject {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private string statName;
        [SerializeField] private StatBaseValueProvider baseValueProvider;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the stat's name.
        /// </summary>
        public string StatName => statName;

        /// <summary>
        /// This property contains the stat's base value provider.
        /// </summary>
        public StatBaseValueProvider BaseValueProvider => baseValueProvider;

        /// <summary>
        /// This property is used to get the stat's stat controller for the given entity.
        /// </summary>
        /// <param name="entity">The entity that you want to get the stat controller for.</param>
        public StatController this[Entity entity] => entity.GetManager<StatManager>()[this];

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try get the stat's value for the given entity.
        /// </summary>
        /// <param name="entity">The entity that you want to get the stat's value from.</param>
        /// <param name="value">The value of the stat for the given entity.</param>
        /// <returns>True if able to get the value, otherwise false.</returns>
        public bool TryGetValue(Entity entity, out int value) {
            value = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            value = statInfo.Value;
            return true;
        }
        
        /// <summary>
        /// This method is used to try get the stat's level for the given entity.
        /// </summary>
        /// <param name="entity">The entity that you want to get the stat's level for.</param>
        /// <param name="level">The level of the stat for the given entity.</param>
        /// <returns>True if able to get the level, otherwise false.</returns>
        public bool TryGetLevel(Entity entity, out int level) {
            level = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            level = statInfo.Level;
            return true;
        }
        
        /// <summary>
        /// This method is used to try get the stat's base level for the given entity.
        /// </summary>
        /// <param name="entity">The entity that you want to get the stat's base level for.</param>
        /// <param name="baseValue">The base level of the stat for the given entity.</param>
        /// <returns>True if able to get the base value, otherwise false.</returns>
        public bool TryGetBaseValue(Entity entity, out int baseValue) {
            baseValue = 0;
            if(!TryGetStatInfo(entity, out var statInfo)) return false;
            baseValue = statInfo.BaseValue;
            return true;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get the stat controller for this stat for the given entity.
        /// </summary>
        /// <param name="entity">The entity that you want to get the stat controller for.</param>
        /// <param name="statController">The stat controller.</param>
        /// <returns>True if able to get the stat controller for the given entity, otherwise false.</returns>
        private bool TryGetStatInfo(Entity entity, out StatController statController) {
            statController = default;
            if(entity == null) return false;
            statController = this[entity];
            return statController is { Initialized: true };
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}
