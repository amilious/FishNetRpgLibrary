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
    [CreateAssetMenu(fileName = "NewQuest", 
        menuName = FishyRpg.QUEST_MENU_ROOT+"New Quest", order = FishyRpg.QUEST_START)]
    public class Quest : AmiliousScriptableObject<Quest> {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the quest!")] private string questName;
        [SerializeField, Tooltip("If true the quest can be abandoned.")] private bool canAbandon = true;
        [SerializeField, Multiline, Tooltip("The quest's description.")] private string description;
        [SerializeField, Tooltip("This can be used to sort quests.")] 
        private List<QuestType> questTypes = new List<QuestType>();
        [SerializeField, Tooltip("The stages of the quest.")] private List<QuestStage> questStages = new();
        [SerializeField, Tooltip("The requirements that must be met before the quest can be accepted")] 
        private List<AbstractRequirement> requirements = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Fields /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This string builder is used for generating keys used for the quest data.
        /// </summary>
        private static readonly StringBuilder KeyBuilder = new (64);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// If this property is true the quest can be abandoned.
        /// </summary>
        public bool CanAbandon => canAbandon;

        /// <summary>
        /// This property is used to get the quest's name.
        /// </summary>
        public string QuestName => questName;

        /// <summary>
        /// This property is used to get the quest's description.
        /// </summary>
        public string Description => description;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to check if the quest belongs to the given quest type.
        /// </summary>
        /// <param name="questType">The quest type that you want to check for.</param>
        /// <returns>True if the quest is of the given quest type, otherwise false.</returns>
        public bool IsQuestType(QuestType questType) => questTypes?.Contains(questType) ?? false;
        
        /// <summary>
        /// This method is used to check if the quest is complete.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <returns>True if the quest is complete, otherwise false.</returns>
        public bool IsComplete(QuestManager manager) {
            //check if the current stage is a valid stage, if not the quest is complete.
            if(!TryGetCurrentStageInfo(manager, out var stageIndex, out var startIndex)) return true;
            while(stageIndex<questStages.Count) {
                //check if the current stage is complete
                var currentComplete = questStages[stageIndex].IsComplete(manager, this, BaseKey, startIndex);
                //if the current stage is not complete than the quest is not complete
                if(!currentComplete) return false;
                //move to the next stage
                manager[Base64Id, questStages.Count, true]++;
                stageIndex++;
                startIndex = StartIndex(stageIndex);
            }
            //if we reach here all stages are complete
            return true;
        }

        /// <summary>
        /// This method is used to clear all quest progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        public void ClearAllProgress(QuestManager manager) {
            var startIndex = 0;
            foreach(var stage in questStages) {
                stage.ClearAllProgress(manager, this, BaseKey, startIndex);
                startIndex += stage.TaskCount;
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

        /// <summary>
        /// This method is used to make sure that the task have a reference to the quest manager.
        /// </summary>
        /// <param name="manager">The quest manager that you want to register with the tasks.</param>
        public void RegisterManager(QuestManager manager) {
            foreach(var stage in questStages) stage.RegisterManager(manager);
        }
        
        /// <summary>
        /// This method is called to send information back to the task that sent it.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="val1">The first value.</param>
        /// <typeparam name="T">The type of the task that sent the data.</typeparam>
        /// <typeparam name="TV1">The type of the first value.</typeparam>
        public void TriggerCallback<T,TV1>(QuestManager manager, TV1 val1) where T : QuestTask<TV1> {
            if(!TryGetCurrentStageInfo(manager,out var stageIndex, out var startIndex)) return;
            var stage = questStages[stageIndex];
            for(var i = 0; i < stage.TaskCount; i++) {
                if(stage[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex,i),val1);
            }
        }

        /// <summary>
        /// This method is called to send information back to the task that sent it.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <typeparam name="T">The type of the task that sent the data.</typeparam>
        /// <typeparam name="TV1">The type of the first value.</typeparam>
        /// <typeparam name="TV2">The type of the second value.</typeparam>
        public void TriggerCallback<T, TV1, TV2>(QuestManager manager, TV1 val1, TV2 val2) 
            where T : QuestTask<TV1, TV2> {
            if(!TryGetCurrentStageInfo(manager,out var stageIndex, out var startIndex)) return;
            var stage = questStages[stageIndex];
            for(var i = 0; i < stage.TaskCount; i++) {
                if(stage[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex,i),val1,val2);
            }
        }

        /// <summary>
        /// This method is called to send information back to the task that sent it.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="val3">The third value.</param>
        /// <typeparam name="T">The type of the task that sent the data.</typeparam>
        /// <typeparam name="TV1">The type of the first value.</typeparam>
        /// <typeparam name="TV2">The type of the second value.</typeparam>
        /// <typeparam name="TV3">The type of the third value.</typeparam>
        public void TriggerCallback<T,TV1,TV2,TV3>(QuestManager manager, TV1 val1, TV2 val2, TV3 val3) 
            where T : QuestTask<TV1,TV2,TV3> {
            if(!TryGetCurrentStageInfo(manager,out var stageIndex, out var startIndex)) return;
            var stage = questStages[stageIndex];
            for(var i = 0; i < stage.TaskCount; i++) {
                if(stage[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex,i),val1,val2,val3);
            }
        }

        /// <summary>
        /// This method is called to send information back to the task that sent it.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="val1">The first value.</param>
        /// <param name="val2">The second value.</param>
        /// <param name="val3">The third value.</param>
        /// <param name="val4">The fourth value.</param>
        /// <typeparam name="T">The type of the task that sent the data.</typeparam>
        /// <typeparam name="TV1">The type of the first value.</typeparam>
        /// <typeparam name="TV2">The type of the second value.</typeparam>
        /// <typeparam name="TV3">The type of the third value.</typeparam>
        /// <typeparam name="TV4">The type of the fourth value.</typeparam>
        public void TriggerCallback<T,TV1,TV2,TV3,TV4>(QuestManager manager, TV1 val1, TV2 val2, TV3 val3, 
            TV4 val4) where T : QuestTask<TV1,TV2,TV3,TV4> {
            if(!TryGetCurrentStageInfo(manager,out var stageIndex, out var startIndex)) return;
            var stage = questStages[stageIndex];
            for(var i = 0; i < stage.TaskCount; i++) {
                if(stage[i] is not T task) continue;
                task.TriggerCallback(manager,this,Key(startIndex,i),val1,val2,val3,val4);
            }
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
                   
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to get the base key with the task index.
        /// </summary>
        /// <param name="startIndex">The start index for the quest stage.</param>
        /// <param name="index">The task index within the stage.</param>
        /// <returns>The base index with the task index.</returns>
        private StringBuilder Key(int startIndex,int index) =>KeyBuilder.Clear()
            .Append(Base64Id).Append(startIndex+index);

        /// <summary>
        /// This method is used to get the base key.
        /// </summary>
        private StringBuilder BaseKey => KeyBuilder.Clear().Append(Base64Id);

        /// <summary>
        /// This method is used to get information related to the current stage.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="stageIndex">The current stage index.</param>
        /// <param name="startIndex">The startIndex for the current stage.</param>
        /// <returns>True if the current stage is valid, otherwise false.</returns>
        private bool TryGetCurrentStageInfo(QuestManager manager, out int stageIndex, out int startIndex) {
            stageIndex = manager[Base64Id];
            startIndex = StartIndex(stageIndex);
            return stageIndex >= 0 && stageIndex < questStages.Count;
        }

        /// <summary>
        /// This method is used to get the start index for the given stage index.
        /// </summary>
        /// <param name="stageIndex">The stage index.</param>
        /// <returns>The start index for the give stage index.</returns>
        private int StartIndex(int stageIndex) {
            if(questStages.Count == 0) return 0;
            stageIndex = Mathf.Clamp(stageIndex, 0, questStages.Count - 1);
            var startIndex = 0;
            for(var i = 0; i <= stageIndex; i++) startIndex += questStages[i].TaskCount;
            return startIndex;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}