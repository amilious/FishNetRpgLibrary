using UnityEngine;

namespace Amilious.FishNetRpg.Resource {
    public class ResourceModifier : Modifier, IResourceModifier {
        
        public override ModifierSystems System => ModifierSystems.ResourceSystem;
        
    }
}