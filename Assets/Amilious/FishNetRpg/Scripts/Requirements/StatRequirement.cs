using Amilious.Core;
using UnityEngine;
using Amilious.Core.Enums;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Statistics;

namespace Amilious.FishNetRpg.Requirements {
    
    [CreateAssetMenu(fileName = "NewStatRequirement", menuName = FishNetRpg.REQUIREMENT_MENU_ROOT+"Stat")]
    public class StatRequirement : AbstractRequirement {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The stat for the requirement.")] 
        private Stat stat;
        [SerializeField, Tooltip("The comparison type for the requirement")] 
        private ComparisonMethod<int> comparisonMethod = new ComparisonMethod<int>();
        [SerializeField, Tooltip("The value to compare the stat with.")] 
        private int compareValue = 0;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the stat that the requirement is for.
        /// </summary>
        public Stat Stat => stat;

        /// <summary>
        /// This property contains the comparision type for the stat requirement.
        /// </summary>
        public ComparisonType ComparisonType => comparisonMethod.ComparisonType;

        /// <summary>
        /// This property contains the comparision value for the requirement.
        /// </summary>
        public int ComparisonValue => compareValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool MeetsRequirement(Entity entity) {
            return entity.TryGetManager(out StatManager manager) && 
                   comparisonMethod.Compare(manager[stat].Value, compareValue);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}