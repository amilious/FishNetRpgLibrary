using UnityEngine;
using Amilious.Core.Enums;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Statistics;

namespace Amilious.FishNetRpg.Requirements {
    
    [CreateAssetMenu(fileName = "NewStatRequirement", menuName = FishNetRpg.STATS_MENU_ROOT+"Stat")]
    public class StatRequirement : AbstractRequirement {

        [SerializeField] private Stat stat;
        [SerializeField] private ComparisonType comparison;
        [SerializeField] private int compareValue;

        /// <inheritdoc />
        public override bool MeetsRequirement(Entity entity) {
            return entity.TryGetManager(out StatManager manager) && 
                   comparison.Compare(manager[stat].Value, compareValue);
        }
        
    }
    
}