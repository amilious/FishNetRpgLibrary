using Amilious.Core.Attributes;
using Amilious.FishNetRpg.Attributes;
using UnityEngine;

namespace Amilious.FishNetRpg.Items {
    
    [ItemEditorBadge("Icons/ItemBadges/SkillBadge")]
    [CreateAssetMenu(fileName = "NewSkill", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Skill", order = 21)]
    public class SkillItem : ActionItem {

        [SerializeField, AmiliousTab("Skill")] 
        private bool passiveSkill;

    }
}