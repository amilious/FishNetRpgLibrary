using UnityEngine;
using FishNetRpgLibrary.Statistics;

namespace FishNetRpgLibrary.Entity {
    
    [RequireComponent(typeof(StatsManager))]
    public class LivingEntity : MonoBehaviour {
        
        public StatsManager Stats { get; private set; }
        
        private void Awake() {
            Stats = GetComponent<StatsManager>();
        }
        
        
    }
    
}