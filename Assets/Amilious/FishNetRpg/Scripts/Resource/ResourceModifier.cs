using UnityEngine;

namespace Amilious.FishNetRpg.Resource {
    public class ResourceModifier : ScriptableObject, IResourceModifier {
        
        public ModifierSystems System => ModifierSystems.ResourceSystem;
        
    }
}