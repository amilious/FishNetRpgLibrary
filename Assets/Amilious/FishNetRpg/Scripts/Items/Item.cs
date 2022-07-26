using UnityEngine;
using System.Linq;
using Amilious.Core;
using System.Collections.Generic;
using Amilious.FishNetRpg.Pickups;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Items {
    
    [CreateAssetMenu(fileName = "NewStandardItem", 
        menuName = FishNetRpg.INVENTORY_MENU_ROOT+"Standard Item", order = 0)]
    public class Item : AmiliousScriptableObject {

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [Header("Item Settings")]
        [SerializeField, Tooltip("The display name for the item.")] 
        private string displayName;
        [SerializeField, Tooltip("The description for the item.")] 
        private string description;
        [SerializeField, Tooltip("The inventory icon for the item.")] 
        private Sprite icon;
        [SerializeField, Tooltip("The max stack size for the item.")] 
        private int maxStack = 1;
        [SerializeField, Tooltip("The pickup for the item.")] 
        private Pickup pickup;
        [SerializeField, Tooltip("Requirements for picking up the item.")]
        private List<AbstractRequirement> pickupRequirements = new();
        [SerializeField, Tooltip("Modifiers that are applied to an entity's when the item is in its inventory.")] 
        private List<Modifier> inventoryModifiers = new();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the item is stackable.
        /// </summary>
        public bool IsStackable => maxStack > 1;
        
        /// <summary>
        /// This property contains the item's display name.
        /// </summary>
        public string DisplayName => displayName;
        
        /// <summary>
        /// This property contains the item's description.
        /// </summary>
        public string Description => description;
        
        /// <summary>
        /// This property contains the item's inventory icon.
        /// </summary>
        public Sprite Icon => icon;
        
        /// <summary>
        /// This property contains the max stack size for this item.
        /// </summary>
        public int MaxStackSize => Mathf.Max(1, maxStack);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Modifier Methods ///////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to apply the modifiers that are applied when the item is in an inventory.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public void ApplyInventoryModifiers(Entity entity) {
            if(inventoryModifiers.Count == 0) return;
            entity.ApplyModifiers(this,inventoryModifiers);
        }

        /// <summary>
        /// This method is used to remove modifiers that are applied when the item is in an inventory.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public void RemoveInventoryModifiers(Entity entity) {
            if(inventoryModifiers.Count == 0) return;
            entity.RemoveModifiers(this,inventoryModifiers);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Requirement Methods /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the entity meets the requirements to pickup this item.
        /// </summary>
        /// <param name="entity">The entity that you want to check.</param>
        /// <returns>True if the entity meets the requirements to pickup this item.</returns>
        public bool MeetsPickupRequirements(Entity entity) {
            return pickupRequirements.Count == 0 || 
                   pickupRequirements.All(x => x.MeetsRequirement(entity));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Pickup Spawning Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to spawn a pickup for this item.
        /// </summary>
        /// <param name="transform">The transform for this item.</param>
        /// <param name="quantity">The quantity of this item.</param>
        /// <returns>The spawned pickup.</returns>
        public Pickup SpawnPickup(Transform transform, int quantity) {
            return SpawnPickup(transform.position, transform.rotation, quantity, transform);
        }

        /// <summary>
        /// This method is used to spawn a pickup for this item.
        /// </summary>
        /// <param name="position">The position of the pickup.</param>
        /// <param name="rotation">The rotation of the pickup.</param>
        /// <param name="quantity">The item quantity.</param>
        /// <param name="parent">A parent transform for the spawned pickup.</param>
        /// <returns>The spawned pickup.</returns>
        public Pickup SpawnPickup(Vector3 position, Quaternion rotation, int quantity, Transform parent = null) {
            if(pickup == null) return null;
            var spawnedPickup = Instantiate(pickup,position,rotation,parent);
            spawnedPickup.Setup(this,quantity);
            return spawnedPickup;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}