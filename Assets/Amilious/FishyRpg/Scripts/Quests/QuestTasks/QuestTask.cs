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
using UnityEngine;
using Amilious.Core;
using Amilious.Core.Extensions;
using Amilious.FishyRpg.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.PlayerLoop;

namespace Amilious.FishyRpg.Quests.QuestTasks {

    /// <summary>
    /// This is the base class for a quest task.
    /// </summary>
    public abstract class QuestTask : AmiliousScriptableObject {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the task.")] 
        private string taskName;
        [SerializeField, Multiline, Tooltip("The description of the task.")] 
        private string description;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Non Serialized Fields //////////////////////////////////////////////////////////////////////////////////
       
        /// <summary>
        /// This list is used to hold the registered quest listeners.
        /// </summary>
        protected readonly List<QuestManager> ActiveManagers = new List<QuestManager>();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property contains the task's name..
        /// </summary>
        public string TaskName => taskName;

        /// <summary>
        /// This property contains the task's description.
        /// </summary>
        public string Description => description;
        
        /// <summary>
        /// This property contains the total number of actions for this task.
        /// </summary>
        public abstract int TotalActions { get; }

        /// <summary>
        /// This property is used to get the active manager's players.
        /// </summary>
        public Player[] Players => ActiveManagers.Select(x => x.Player).ToArray();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to add the manager.
        /// </summary>
        /// <param name="questManager">The manager that you want to add.</param>
        public void AddManager(QuestManager questManager) {
            if(ActiveManagers.Contains(questManager)) return;
            ActiveManagers.Add(questManager);
        }
        
        /// <summary>
        /// This method is used to check if the task is complete.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>True if the task is complete, otherwise false.</returns>
        public bool IsComplete(QuestManager manager, Quest quest, StringBuilder baseKey) =>
            TotalActions <= CompletedActions(manager, quest, baseKey);

        /// <summary>
        /// This method is used to get the number of completed actions.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>The number of completed actions.</returns>
        public int CompletedActions(QuestManager manager, Quest quest, StringBuilder baseKey) =>
            GetCompletedActions(manager, quest, baseKey.Append(Base64Id));

        /// <summary>
        /// This method is used to clear all of the tasks progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="baseKey">The base key.</param>
        public void ClearAllProgress(QuestManager manager, Quest quest, StringBuilder baseKey) =>
            ClearProgress(manager, quest,baseKey.Append(Base64Id));
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to clear all of the tasks progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="baseKey">The base key.</param>
        protected abstract void ClearProgress(QuestManager manager, Quest quest, StringBuilder baseKey);

        /// <summary>
        /// This method is used to get the number of completed actions.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>The number of completed actions.</returns>
        protected abstract int GetCompletedActions(QuestManager manager, Quest quest, StringBuilder baseKey);

        /// <summary>
        /// This method is called when a task is active at the rate set for the quest manager.
        /// </summary>
        /// <param name="manager">The calling manager.</param>
        /// <param name="quest">The quest that has the task.</param>
        /// <param name="baseKey">The base key for the task.</param>
        public virtual void Update(QuestManager manager, Quest quest, StringBuilder baseKey) { }

        /// <inheritdoc />
        protected override void BeforeSerialize() {
            if(string.IsNullOrWhiteSpace(taskName)) taskName = name.SplitCamelCase();
            if(string.IsNullOrWhiteSpace(description)) description = FishyRpg.DEFAULT_DESCRIPTION;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
    
    public abstract class QuestTask<T, T2, T3, T4> : QuestTask {

        public void SendData<TS>(TS sender, T val1, T2 val2, T3 val3, T4 val4) where TS : QuestTask<T, T2, T3, T4> {
            foreach(var manager in ActiveManagers) {
                if(manager == null) continue;
                manager.TriggerCallback<TS,T,T2,T3,T4>(val1,val2,val3,val4);
            }
        }

        public void TriggerCallback(QuestManager manager, Quest quest, StringBuilder baseKey, T val,
            T2 val2, T3 val3, T4 val4) => Callback(manager, quest, baseKey.Append(Base64Id), val, val2, val3, val4);
        
        protected abstract void Callback(QuestManager manager, Quest quest, StringBuilder baseKey, T val, 
            T2 val2, T3 val3, T4 val4);
    }
    
    public abstract class QuestTask<T, T2, T3> : QuestTask {

        public void SendData<TS>(TS sender, T val1, T2 val2, T3 val3) where TS : QuestTask<T, T2, T3> {
            foreach(var manager in ActiveManagers) {
                if(manager == null) continue;
                manager.TriggerCallback<TS,T,T2,T3>(val1,val2,val3);
            }
        }
        
        public void TriggerCallback(QuestManager manager, Quest quest, StringBuilder baseKey, T val,
            T2 val2, T3 val3) => Callback(manager, quest, baseKey.Append(Base64Id), val, val2, val3);
        
        protected abstract void Callback(QuestManager manager, Quest quest, StringBuilder baseKey, T val, 
            T2 val2, T3 val3);
    }
    
    public abstract class QuestTask<T, T2> : QuestTask {

        public void SendData<TS>(TS sender, T val1, T2 val2) where TS : QuestTask<T, T2> {
            foreach(var manager in ActiveManagers) {
                if(manager == null) continue;
                manager.TriggerCallback<TS,T,T2>(val1,val2);
            }
        }
        
        public void TriggerCallback(QuestManager manager, Quest quest, StringBuilder baseKey, T val,
            T2 val2) => Callback(manager, quest, baseKey.Append(Base64Id), val, val2);
        
        protected abstract void Callback(QuestManager manager, Quest quest, StringBuilder baseKey, T val, 
            T2 val2);
        
    }
    
    public abstract class QuestTask<T> : QuestTask {

        public void SendData<TS>(TS sender, T val1) where TS : QuestTask<T> {
            foreach(var manager in ActiveManagers) {
                if(manager == null) continue;
                manager.TriggerCallback<TS,T>(val1);
            }
        }
        
        public void TriggerCallback(QuestManager manager, Quest quest, StringBuilder baseKey, T val) =>
            Callback(manager, quest, baseKey.Append(Base64Id), val);
        
        protected abstract void Callback(QuestManager manager, Quest quest, StringBuilder baseKey, T val);

    }
    
}