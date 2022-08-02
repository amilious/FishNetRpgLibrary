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

using System;
using System.Text;
using UnityEngine;
using Amilious.Core.Extensions;
using System.Collections.Generic;
using Amilious.FishyRpg.Quests.QuestTasks;

// ReSharper disable LoopCanBeConvertedToQuery
namespace Amilious.FishyRpg.Quests {
    
    /// <summary>
    /// This class is used as a stage for a quest.
    /// </summary>
    [Serializable]
    public class QuestStage : ISerializationCallbackReceiver {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the quest stage.")] private string stageName;
        [SerializeField, Multiline, Tooltip("The stage's description.")] private string description;
        [SerializeField, Tooltip("The quest tasks for this stage.")] private List<QuestTask> questTasks = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the stage's name.
        /// </summary>
        public string StageName => stageName;

        /// <summary>
        /// This property contains the stage's description.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// This property is used to get the total number of tasks for the stage.
        /// </summary>
        public int TaskCount => questTasks.Count;

        /// <summary>
        /// This property is used to get the task with the given index.
        /// </summary>
        /// <param name="index">The task's local stage index.</param>
        public QuestTask this[int index] => questTasks[index];
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when a task is active at the rate set for the quest manager.
        /// </summary>
        /// <param name="manager">The calling manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="delta">The time since the last update.</param>
        /// <param name="baseKey">The base key for the task.</param>
        /// <param name="startIndex">The starting index.</param>
        public void Update(QuestManager manager, Quest quest, float delta, StringBuilder baseKey, int startIndex) {
            for(var i=0;i<questTasks.Count;i++) questTasks[i].Update(manager,quest,delta,baseKey.Append(startIndex+i));
        }
        
        /// <summary>
        /// This method is used to notify the tasks of a manager that is using the task.
        /// </summary>
        /// <param name="manager">The quest manager that is using the task.</param>
        public void AddManager(QuestManager manager) {
            foreach(var task in questTasks) task.AddManager(manager);
        }

        /// <summary>
        /// This method is used to check if all of the tasks in the stage are complete.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest.</param>
        /// <param name="baseKey">The base key.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <returns>True if all of the stage's tasks have been completed, otherwise false.</returns>
        public bool IsComplete(QuestManager manager, Quest quest, StringBuilder baseKey, int startIndex) {
            if(questTasks.Count == 0) return true;
            for(var i = 0; i < questTasks.Count; i++)
                if(!questTasks[i].IsComplete(manager, quest, baseKey.Append(startIndex+i))) return false;
            return true;
        }

        /// <summary>
        /// This method is used to clear the quest's progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest that the stage belongs to.</param>
        /// <param name="baseKey">The base key without the index.</param>
        /// <param name="startIndex">The start index.</param>
        public void ClearAllProgress(QuestManager manager, Quest quest, StringBuilder baseKey, int startIndex) {
            if(questTasks.Count == 0) return;
            for(var i = 0; i < questTasks.Count; i++) 
                questTasks[i].ClearAllProgress(manager, quest,baseKey.Append(startIndex+i));
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is called before serialization so we can use it to insure default values.
        /// </summary>
        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            if(string.IsNullOrWhiteSpace(stageName)) stageName = GetType().SplitCamelCase();
            if(string.IsNullOrWhiteSpace(description)) description = FishyRpg.DEFAULT_DESCRIPTION;
        }

        /// <summary>
        /// This method is not used but it is required for the callback receiver.
        /// </summary>
        void ISerializationCallbackReceiver.OnAfterDeserialize() { }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}