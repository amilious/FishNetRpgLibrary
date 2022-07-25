using UnityEngine;

namespace Amilious.FishNetRpg.Modifiers {
    
    [CreateAssetMenu(fileName = "NewExperienceModifier", menuName = FishNetRpg.MODIFIERS_MENU_ROOT + "Experience")]
    public class ExperienceModifier : Modifier, IExperienceModifier {
        public override Systems System => Systems.LevelSystem;
    }
}