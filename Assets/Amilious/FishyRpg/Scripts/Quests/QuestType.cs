using UnityEngine;

namespace Amilious.FishyRpg.Quests {
    
    [CreateAssetMenu(fileName = "NewQuestType", 
        menuName = FishyRpg.QUEST_MENU_ROOT+"New Quest Type", order = FishyRpg.QUEST_START+1)]
    public class QuestType : ScriptableObject { }
}