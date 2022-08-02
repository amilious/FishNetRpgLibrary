/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System.Collections.Generic;
using System.Linq;
using Amilious.Core.Attributes;
using Amilious.FishyRpg.Attributes;
using Amilious.FishyRpg.Entities;
using Amilious.FishyRpg.Modifiers;
using Amilious.FishyRpg.Requirements;
using UnityEngine;

namespace Amilious.FishyRpg.Items {
    
    [ItemEditorBadge("Icons/ItemBadges/ActionBadge")]
    [CreateAssetMenu(fileName = "NewActionItem", 
        menuName = FishyRpg.ITEM_MENU_ROOT+"New Action Item", order = FishyRpg.ITEM_START+MENU_ACTION)]
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