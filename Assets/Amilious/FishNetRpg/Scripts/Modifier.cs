using Amilious.Core;

namespace Amilious.FishNetRpg {
    public abstract class Modifier : AmiliousScriptableObject {
        
        public abstract ModifierSystems System { get; }
        
    }
}