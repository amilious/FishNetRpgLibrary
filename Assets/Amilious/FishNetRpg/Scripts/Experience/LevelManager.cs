using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using FishNet.Object;
using UnityEngine;

namespace Amilious.FishNetRpg.Experience {
    
    [RequireComponent(typeof(Entity),typeof(ModifierManager))]
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Level Manager")]
    public class LevelManager : NetworkBehaviour {
        
    }
}