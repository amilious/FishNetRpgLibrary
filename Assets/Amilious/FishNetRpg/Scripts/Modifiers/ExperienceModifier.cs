namespace Amilious.FishNetRpg.Modifiers {
    public class ExperienceModifier : Modifier, IExperienceModifier {
        public override Systems System => Systems.LevelSystem;
    }
}