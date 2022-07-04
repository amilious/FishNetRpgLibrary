using Amilious.FishNetRpg.Statistics;
using FishNet.Object;
using UnityEngine;

namespace Amilious.FishNetRpg.Entities {
    
    [RequireComponent(typeof(StatManager))]
    public class Entity : NetworkBehaviour {
        
        public StatManager StatManager { get; private set; }

        private void Awake() {
            StatManager = GetComponent<StatManager>();
        }
    }
    
}
