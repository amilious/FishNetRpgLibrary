using Amilious.FishNetRpg.Entities;
using Amilious.FishNetRpg.Modifiers;
using FishNet.Object;
using UnityEngine;

namespace Amilious.FishNetRpg.Resource {
    
    [RequireComponent(typeof(Entity),typeof(ModifierManager))]
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Resource Manager")]
    public class ResourceManager : NetworkBehaviour {
        
    }
}