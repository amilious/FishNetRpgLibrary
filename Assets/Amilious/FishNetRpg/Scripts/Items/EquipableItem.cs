using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Amilious.Core.Attributes;
using Amilious.FishNetRpg.Attributes;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Equipment;
using Amilious.FishNetRpg.Modifiers;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Items {
    
    [ItemEditorBadge("Icons/ItemBadges/EquipmentBadge")]
    [CreateAssetMenu(fileName = "NewEquipableItem", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Equipable Item", order = 21)]
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