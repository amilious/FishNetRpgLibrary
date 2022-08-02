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
using Amilious.FishyRpg.Inventory;
using Amilious.FishyRpg.Modifiers;
using Amilious.FishyRpg.Requirements;
using UnityEngine;

namespace Amilious.FishyRpg.Items {
    
    [ItemEditorBadge("Icons/ItemBadges/EquipmentBadge")]
    [CreateAssetMenu(fileName = "NewEquipableItem", 
        menuName = FishyRpg.ITEM_MENU_ROOT+"New Equipable Item", order = FishyRpg.ITEM_START+MENU_EQUIP)]
    public class EquipableItem : Item {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, AmiliousTab("Equipable"), Tooltip("The places that this item can be equipped.")] 
        private List<EquipmentSlotType> allowedEquipmentSlots = new();
        [SerializeField, AmiliousTab("Equipable"),Tooltip("Requirements that must be satisfied to equip this item.")] 
        private List<AbstractRequirement> equipRequirements = new();
        [SerializeField, AmiliousTab("Equipable"),Tooltip("Modifiers that will be applied when this item is equipped.")] 
        private List<Modifier> equipAppliedModifiers = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        public IReadOnlyList<EquipmentSlotType> AllowedEquipmentSlots => allowedEquipmentSlots;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Equip Modifier Methods /////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to apply the equipped modifiers.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public void ApplyEquipModifiers(Entity entity) {
            if(equipAppliedModifiers.Count == 0) return;
            entity.ApplyModifiers(this,equipAppliedModifiers);
        }

        /// <summary>
        /// This method is used to remove the equipped modifiers.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be removed from.</param>
        public void RemoveEquipModifiers(Entity entity) {
            if(equipAppliedModifiers.Count == 0) return;
            entity.RemoveModifiers(this,equipAppliedModifiers);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Requirement Methods ////////////////////////////////////////////////////////////////////////////////////
        
        
        public bool MeetsEquipRequirements(Entity entity) {
            return equipRequirements.Count == 0 || 
                   equipRequirements.All(x => x.MeetsRequirement(entity));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}