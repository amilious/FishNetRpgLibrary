using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Equipment;
using Amilious.FishNetRpg.Modifiers;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Items {
    
    [CreateAssetMenu(fileName = "NewEquippableItem", 
        menuName = FishNetRpg.INVENTORY_MENU_ROOT+"Equippable Item", order = 1)]
    public class EquippableItem : Item, IRequirementProvider {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Header("Equipment Settings")]
        [SerializeField, Tooltip("The places that this item can be equipped.")] 
        private List<EquipmentSlotType> allowedLocations = new();
        [SerializeField,Tooltip("Requirements that must be satisfied to equip this item.")] 
        private List<AbstractRequirement> equipRequirements = new();
        [SerializeField,Tooltip("Modifiers that will be applied when this item is equipped.")] 
        private List<Modifier> equippedModifiers = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Equip Modifier Methods /////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to apply the equipped modifiers.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public void ApplyEquippedModifiers(Entity entity) {
            if(equippedModifiers.Count == 0) return;
            entity.ApplyModifiers(this,equippedModifiers);
        }

        /// <summary>
        /// This method is used to remove the equipped modifiers.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be removed from.</param>
        public void RemoveEquippedModifiers(Entity entity) {
            if(equippedModifiers.Count == 0) return;
            entity.RemoveModifiers(this,equippedModifiers);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Requirement Methods ////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public bool MeetsAllRequirements(Entity entity) {
            return equipRequirements.Count == 0 || 
                   equipRequirements.All(x => x.MeetsRequirement(entity));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}