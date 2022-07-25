using Amilious.FishNetRpg.Experience;

namespace Amilious.FishNetRpg.Modifiers {
    public class LevelModifier : Modifier, ILevelModifier {
        public override Systems System => Systems.LevelSystem;
    }
}