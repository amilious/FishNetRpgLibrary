using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using Amilious.FishyRpg.Quests.QuestTasks;

namespace Amilious.FishyRpg.Quests {
    
    [Serializable]
    public class QuestStage {
        
        [SerializeField]private List<QuestTask> questTasks = new ();
        
        private static readonly StringBuilder KeyBuilder = new (64);

        private StringBuilder Key(string baseKey, int startIndex, int index) => 
            KeyBuilder.Clear().Append(baseKey).Append(startIndex+index);
        
        public void AddManager(QuestManager manager) {
            foreach(var quest in questTasks) quest.AddManager(manager);
        }

        public int TaskCount => questTasks.Count;

        public QuestTask this[int index] => questTasks[index];

        public bool IsComplete(QuestManager manager, string base64Id, int startIndex) {
            if(questTasks.Count == 0) return true;
            for(var i = 0; i < questTasks.Count; i++)
                if(!questTasks[i].IsComplete(manager, Key(base64Id, startIndex, i)))
                    return false;
            return true;
        }

        public void ClearAllProgress(QuestManager manager, string base64Id, int startIndex) {
            if(questTasks.Count == 0) return;
            for(var i = 0; i < questTasks.Count; i++) 
                questTasks[i].ClearAllProgress(manager, Key(base64Id,startIndex,i));
        }
    }
}