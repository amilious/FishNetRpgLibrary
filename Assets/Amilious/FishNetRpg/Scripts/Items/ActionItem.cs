using System.Linq;
using UnityEngine;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using Amilious.FishNetRpg.Attributes;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Items {
    
    [ItemEditorBadge("ItemBadges/ActionBadge")]
    [CreateAssetMenu(fileName = "NewActionItem", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Action Item", order = 21)]
    public class ActionItem : Item {

        [SerializeField, AmiliousTab("Action"), Tooltip("If true the item will be consumed when it is used.")] 
        private bool consumable = false;
        [SerializeField, AmiliousTab("Action"), Tooltip("The cool down time for this action.")]
        private float cooldown = 0f;
        [SerializeField, AmiliousTab("Action"), Tooltip("An optional groups to apply the cool down to.")]
        private List<CooldownGroup> cooldownGroups = new List<CooldownGroup>();
        [SerializeField, AmiliousTab("Action"), Tooltip("The requirements must be met for the action to be triggered.")]
        private List<AbstractRequirement> actionRequirements = new List<AbstractRequirement>();
        [SerializeField, AmiliousTab("Action"), Tooltip("These modifiers will be applied when the action is triggered.")]
        private List<Modifier> triggerAppliedModifiers = new List<Modifier>();
        
        public bool Consumable => consumable;

        public float Cooldown => cooldown;

        public IReadOnlyList<CooldownGroup> CooldownGroup => cooldownGroups;

        public virtual void TriggerAction(Entity triggeringEntity) {
            if(triggeringEntity == null) return;
            triggeringEntity.ApplyModifiers(this,triggerAppliedModifiers);
        }
        
        public bool MeetsActionRequirements(Entity entity) {
            return actionRequirements.Count == 0 || 
                   actionRequirements.All(x => x.MeetsRequirement(entity));
        }

    }
}