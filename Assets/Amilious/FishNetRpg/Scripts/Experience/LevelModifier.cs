using UnityEngine;

namespace Amilious.FishNetRpg.Experience {
    public class LevelModifier : Modifier, ILevelModifier {
        public override ModifierSystems System => ModifierSystems.LevelSystem;
    }
}