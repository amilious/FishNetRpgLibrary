using UnityEngine;

namespace Amilious.FishNetRpg.Experience {
    public class LevelModifier : ScriptableObject, ILevelModifier {
        public ModifierSystems System => ModifierSystems.LevelSystem;
    }
}