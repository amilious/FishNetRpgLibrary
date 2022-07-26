using Amilious.Core;
using Amilious.FishNetRpg.Pickups;
using UnityEngine;

namespace Amilious.FishNetRpg.Items {
    
    [CreateAssetMenu(fileName = "NewStandardItem", 
        menuName = FishNetRpg.INVENTORY_MENU_ROOT+"Standard Item", order = 0)]
    public class Item : AmiliousScriptableObject {

        [SerializeField] private string displayName;
        [SerializeField] private string description;
        [SerializeField] private Sprite icon;
        [SerializeField] private int maxStack = 1;
        [SerializeField] private Pickup pickup;

        private bool IsStackable => maxStack > 1;
        private string DisplayName => displayName;
        private string Description => description;
        private Sprite Icon => icon;
        private int MaxStackSize => Mathf.Max(1, maxStack);

        public Pickup SpawnPickup(Transform transform, int quantity) {
            return SpawnPickup(transform.position, quantity, transform);
        }

        public Pickup SpawnPickup(Vector3 position, int quantity, Transform parent = null) {
            if(pickup == null) return null;
            var spawnedPickup = Instantiate(pickup,position,Quaternion.identity,parent);
            spawnedPickup.Setup(this,quantity);
            return spawnedPickup;
        }
        
    }
    
}