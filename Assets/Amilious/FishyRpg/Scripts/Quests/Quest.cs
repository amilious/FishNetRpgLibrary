/*//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
   _____            .__ .__   .__                             ____       ___________                __                  
  /  _  \    _____  |__||  |  |__|  ____   __ __  ______     /  _ \      \__    ___/____  ___  ____/  |_  ____   ______ 
 /  /_\  \  /     \ |  ||  |  |  | /  _ \ |  |  \/  ___/     >  _ </\      |    | _/ __ \ \  \/  /\   __\/  _ \ /  ___/ 
/    |    \|  Y Y  \|  ||  |__|  |(  <_> )|  |  /\___ \     /  <_\ \/      |    | \  ___/  >    <  |  | (  <_> )\___ \  
\____|__  /|__|_|  /|__||____/|__| \____/ |____//____  >    \_____\ \      |____|  \___  >/__/\_ \ |__|  \____//____  > 
        \/       \/                                  \/            \/                  \/       \/                  \/

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  Website:        http://www.amilious,com         Unity Asset Store: https://assetstore.unity.com/publishers/62511  //
//  Discord Server: https://discord.gg/SNqyDWu            Copyright© Amilious, Textos since 2022                      //                    
//  This code is part of an asset on the unity asset store. If you did not get this from the asset store you are not  //
//  using it legally. Check the asset store or join the discord for the license that applies for this script.         //
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////*/

using System.Text;
using System.Linq;
using UnityEngine;
using Amilious.Core;
using Amilious.FishyRpg.Entities;
using System.Collections.Generic;
using Amilious.FishyRpg.Requirements;
using Amilious.FishyRpg.Quests.QuestTasks;

namespace Amilious.FishyRpg.Quests {
    
    /// <summary>
    /// This class is used to represent a quest.
    /// </summary>
    [CreateAssetMenu(fileName = "NewQuest", menuName = FishNetRpg.QUEST_MENU_ROOT+"Quest")]
    public class Quest : AmiliousScriptableObject<Quest> {
        
        private static readonly StringBuilder KeyBuilder = new (64);

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private string questName;
        [SerializeField, Multiline] private string questDescription;
        [SerializeField] private List<QuestStage> questStages = new();
        [SerializeField] private List<AbstractRequirement> requirements = new ();
        [SerializeField] private bool canAbandon = true;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// If this property is true the quest can be abandoned.
        /// </summary>
        public bool CanAbandon => canAbandon;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the quest is complete.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <returns>True if the quest is complete, otherwise false.</returns>
        public bool IsComplete(QuestManager manager) {
            //check if the current stage is complete
            if(!GetCurrentStage(manager, out var startIndex).IsComplete(manager, Base64Id, startIndex)) return false;
            if(startIndex + 1 == questStages.Count) return true;
            manager[Base64Id, questStages.Count,true]++;
            //loop to check for complete
            while(GetCurrentStage(manager, out startIndex).IsComplete(manager, Base64Id, startIndex)) {
                if(startIndex + 1 == questStages.Count) return true;
                manager[Base64Id, questStages.Count,true]++;
            }
            return false;
        }

        /// <summary>
        /// This method is used to clear all quest progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        public void ClearAllProgress(QuestManager manager) {
            var startIndex = 0;
            for(var i = 0; i < questStages.Count; i++) {
                questStages[i].ClearAllProgress(manager, Base64Id, startIndex);
                startIndex += questStages[i].TaskCount;
            }
        }
        
        /// <summary>
        /// This method is used to check if an entity meets all of the requirements for the quest.
        /// </summary>
        /// <param name="player">The player that you are checking for.</param>
        /// <returns>True if the player meets all of the requirements, otherwise false.</returns>
        public bool MeetsAllRequirements(Player player) {
            return requirements.All(requirement => requirement.MeetsRequirement(player));
        }

        public void AddManager(QuestManager manager) {
            foreach(var stage in questStages) stage.AddManager(manager);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Public Virtual Methods /////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when the quest is taken.
        /// </summary>
        /// <param name="manager">The player that took the quest's <see cref="QuestManager"/>.</param>
        public virtual void OnQuestTaken(QuestManager manager) { }

        /// <summary>
        /// This method is called when the quest is complete.
        /// </summary>
        /// <param name="manager">The player that took the quest's <see cref="QuestManager"/>.</param>
        public virtual void OnQuestComplete(QuestManager manager) { }
        
        /// <summary>
        /// This method is called when the quest is updated.
        /// </summary>
        /// <param name="manager">The player that took the quest's <see cref="QuestManager"/>.</param>
        public virtual void OnQuestUpdated(QuestManager manager) { }
        
        /// <summary>
        /// This method is called when the quest is abandoned.
        /// </summary>
        /// <param name="manager">The player that took the quest's <see cref="QuestManager"/>.</param>
        public virtual void OnQuestAbandoned(QuestManager manager) { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// The method is used to get Key.
        /// </summary>
        private StringBuilder Key(int index) =>KeyBuilder.Clear().Append(Base64Id).Append(index);

        public void TriggerCallback<T, TV1>(QuestManager manager, TV1 val1) where T : QuestTask<TV1> {
            var tasks = GetCurrentStage(manager, out var startIndex);
            for(var i = 0; i < tasks.TaskCount; i++) {
                if(tasks[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex+i),val1);
            }
        }
        
        public void TriggerCallback<T, TV1, TV2>(QuestManager manager, TV1 val1, TV2 val2) 
            where T : QuestTask<TV1, TV2> {
            var tasks = GetCurrentStage(manager, out var startIndex);
            for(var i = 0; i < tasks.TaskCount; i++) {
                if(tasks[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex+i),val1,val2);
            }
        }
        
        public void TriggerCallback<T, TV1, TV2, TV3>(QuestManager manager, TV1 val1, TV2 val2, TV3 val3) 
            where T : QuestTask<TV1, TV2, TV3> {
            var tasks = GetCurrentStage(manager, out var startIndex);
            for(var i = 0; i < tasks.TaskCount; i++) {
                if(tasks[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex+i),val1,val2,val3);
            }
        }
        
        public void TriggerCallback<T, TV1, TV2, TV3, TV4>(QuestManager manager, TV1 val1, TV2 val2, TV3 val3, 
            TV4 val4) where T : QuestTask<TV1, TV2, TV3,TV4> {
            var tasks = GetCurrentStage(manager, out var startIndex);
            for(var i = 0; i < tasks.TaskCount; i++) {
                if(tasks[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex+i),val1,val2,val3,val4);
            }
        }

        private QuestStage GetCurrentStage(QuestManager manager, out int startIndex) {
            var stage = manager[Base64Id, questStages.Count,true];
            startIndex = 0;
            for(var i = 0; i <= stage; i++) startIndex += questStages[i].TaskCount;
            return questStages[stage];
        }
        
    }
}