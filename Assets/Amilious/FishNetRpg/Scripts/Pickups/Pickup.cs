using System;
using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Items;
using UnityEngine;

namespace Amilious.FishNetRpg.Pickups {
    
    public class Pickup : MonoBehaviour {
        
        public Item Item { get; private set; }
        public int Quantity { get; private set; }
        
        public Metadata Metadata { get; private set; }

        public void Setup(Item item, int quantity, Metadata metaData) {
            Item = item;
            Quantity = quantity;
            Metadata = metaData;
        }

        public bool PickupItem(Entity entity) {
            throw new NotImplementedException();
        }
        
    }
    
}