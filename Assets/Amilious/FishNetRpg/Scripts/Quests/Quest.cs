using System.Linq;
using UnityEngine;
using Amilious.Core;
using System.Collections.Generic;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Quests {
    
    [CreateAssetMenu(fileName = "NewQuest", menuName = FishNetRpg.QUEST_MENU_ROOT+"Quest")]
    public class Quest : AmiliousScriptableObject, IRequirementProvider {

        [SerializeField] private string questName;
        [SerializeField] private List<AbstractRequirement> requirements = new ();

        public bool MeetsAllRequirements(Entity entity) {
            return requirements.All(requirement => requirement.MeetsRequirement(entity));
        }
        
    }
}