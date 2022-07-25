using Amilious.FishNetRpg.Experience;
using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    [CreateAssetMenu(fileName = "NewLevelModifier", menuName = FishNetRpg.MODIFIERS_MENU_ROOT + "Level")]
    public class LevelModifier : Modifier, ILevelModifier {
        public override Systems System => Systems.LevelSystem;
    }
}