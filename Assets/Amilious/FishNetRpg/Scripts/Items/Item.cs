using UnityEngine;
using System.Linq;
using Amilious.Core;
using System.Collections.Generic;
using Amilious.Core.Extensions;
using Amilious.FishNetRpg.Pickups;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using Amilious.FishNetRpg.Requirements;

namespace Amilious.FishNetRpg.Items {
    
    [CreateAssetMenu(fileName = "NewStandardItem", 
        menuName = FishNetRpg.ITEM_MENU_ROOT+"New Standard Item", order = 20)]
    public class Item : AmiliousScriptableObject {

        private const string DEFAULT_DESCRIPTION = "No description!";

        #region Inspector Fields ///////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The display name for the item.")] 
        private string displayName;
        [SerializeField, Tooltip("The description for the item."), TextArea] 
        private string description = DEFAULT_DESCRIPTION;
        [SerializeField, Tooltip("The inventory icon for the item.")]
        private Sprite icon;
        [SerializeField, Tooltip("The max stack size for the item.")] 
        private int maxStackSize = 1;
        [SerializeField, Tooltip("The pickup for the item.")] 
        private Pickup pickup;
        [SerializeField, Tooltip("The item's rarity.")] 
        private ItemRarity rarity;
        [SerializeField, Tooltip("The item's weight.")]
        private float weight = 0f;
        [SerializeField]
        [Tooltip("If true this item can be traded to another player, otherwise the item can't be traded.")]
        private bool canBeTraded = true;
        [SerializeField, Tooltip("Requirements for picking up the item.")]
        private List<AbstractRequirement> pickupRequirements = new();
        [SerializeField, Tooltip("Modifiers that are applied to an entity's when the item is in its inventory.")] 
        private List<Modifier> inventoryAppliedModifiers = new();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property is true if the item is stackable.
        /// </summary>
        public virtual bool IsStackable => maxStackSize > 1;
        
        /// <summary>
        /// This property contains the item's display name.
        /// </summary>
        public virtual string DisplayName => displayName;
        
        /// <summary>
        /// This property contains the item's description.
        /// </summary>
        public virtual string Description => description;

        /// <summary>
        /// This property contains the item's inventory icon.
        /// </summary>
        public virtual Sprite Icon => icon;

        /// <summary>
        /// This property contains the item's weight.
        /// </summary>
        public virtual float Weight => weight;

        /// <summary>
        /// This property contains the item's rarity.
        /// </summary>
        public virtual ItemRarity Rarity => rarity;
        
        /// <summary>
        /// This property contains the max stack size for this item.
        /// </summary>
        public virtual int MaxStackSize => Mathf.Max(1, maxStackSize);

        /// <summary>
        /// If this property is true this item can be traded to another player, otherwise the item can't be traded.
        /// </summary>
        public virtual bool CanBeTraded => canBeTraded;

        /// <inheritdoc />
        public override bool NeedsToBeLoadableById => true;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Modifier Methods ///////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to apply the modifiers that are applied when the item is in an inventory.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public virtual void ApplyInventoryModifiers(Entity entity) {
            if(inventoryAppliedModifiers.Count == 0) return;
            entity.ApplyModifiers(this,inventoryAppliedModifiers);
        }

        /// <summary>
        /// This method is used to remove modifiers that are applied when the item is in an inventory.
        /// </summary>
        /// <param name="entity">The entity that the modifiers should be added to.</param>
        public virtual void RemoveInventoryModifiers(Entity entity) {
            if(inventoryAppliedModifiers.Count == 0) return;
            entity.RemoveModifiers(this,inventoryAppliedModifiers);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Event Calls ////////////////////////////////////////////////////////////////////////////////////////////
                  
        public virtual void OnPickupItem(Entity entity) { }

        public virtual void OnDropItem(Entity entity) { }
        
        public virtual void OnDestroyItem(Entity entity){}

        #endregion
        
        #region Public Requirement Methods /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the entity meets the requirements to pickup this item.
        /// </summary>
        /// <param name="entity">The entity that you want to check.</param>
        /// <returns>True if the entity meets the requirements to pickup this item.</returns>
        public virtual bool MeetsPickupRequirements(Entity entity) {
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
        /// <param name="metadata">The item's metadata.</param>
        /// <returns>The spawned pickup.</returns>
        public virtual Pickup SpawnPickup(Transform transform, int quantity, Metadata metadata = null) {
            return SpawnPickup(transform.position, transform.rotation, quantity, metadata, transform);
        }

        /// <summary>
        /// This method is used to spawn a pickup for this item.
        /// </summary>
        /// <param name="position">The position of the pickup.</param>
        /// <param name="rotation">The rotation of the pickup.</param>
        /// <param name="quantity">The item quantity.</param>
        /// <param name="metadata">The item's metadata</param>
        /// <param name="parent">A parent transform for the spawned pickup.</param>
        /// <returns>The spawned pickup.</returns>
        public virtual Pickup SpawnPickup(Vector3 position, Quaternion rotation, int quantity, 
            Metadata metadata = null, Transform parent = null) {
            if(pickup == null) return null;
            var spawnedPickup = Instantiate(pickup,position,rotation,parent);
            spawnedPickup.Setup(this,quantity,metadata);
            return spawnedPickup;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Static Fields //////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to get an instance of an item by it's id.
        /// </summary>
        /// <param name="id">The items id.</param>
        /// <returns>The item with the given id or null if the item was not found.</returns>
        public static Item FromId(long id) {
            return ItemLoader.LoadFromId(id);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
                                  
        protected override void BeforeSerialize() {
            if(string.IsNullOrWhiteSpace(displayName)) displayName = name.SplitCamelCase();
            if(string.IsNullOrWhiteSpace(description)) description = DEFAULT_DESCRIPTION;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods //////////////////////////////////////////////////////////////////////////////////////////
        
        private void OnEnable() {
            if(!IsInResourceFolder&&!string.IsNullOrWhiteSpace(ResourcePath)) 
                Debug.LogErrorFormat("Item \"{0}\" is not in a Resources/ folder!", ResourcePath);
        }

        private void Awake() => OnEnable();

        private void OnValidate() => OnEnable();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}