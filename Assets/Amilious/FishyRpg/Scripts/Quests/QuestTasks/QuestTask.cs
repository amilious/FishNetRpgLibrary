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

using UnityEngine;
using Amilious.Core;
using Amilious.FishyRpg.Entities;

namespace Amilious.FishyRpg.Quests.QuestTasks {
    
    /// <summary>
    /// This is the base class for a quest task.
    /// </summary>
    public abstract class QuestTask : AmiliousScriptableObject {

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField, Tooltip("The name of the task.")] 
        private string taskName;
        [SerializeField, Multiline, Tooltip("The description of the task.")] 
        private string taskDescription;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the total number of actions for this task.
        /// </summary>
        public abstract int TotalActions { get; }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to check if the task is complete.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>True if the task is complete, otherwise false.</returns>
        public bool IsComplete(QuestManager manager, string baseKey) =>
            TotalActions <= CompletedActions(manager, baseKey);

        /// <summary>
        /// This method is used to get the number of completed actions.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>The number of completed actions.</returns>
        public int CompletedActions(QuestManager manager, string baseKey) =>
            GetCompletedActions(manager, baseKey + CachedIdString);

        /// <summary>
        /// This method is used to clear all of the tasks progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        public void ClearAllProgress(QuestManager manager, string baseKey) =>
            ClearProgress(manager, baseKey + CachedIdString);

        /// <summary>
        /// This method is called when an entity dies.
        /// </summary>
        /// <param name="died">The entity that died.</param>
        /// <param name="killer">The entities killer.</param>
        /// <param name="quest">The quest.</param>
        /// <param name="questManager">The quest manager.</param>
        /// <param name="baseKey">The base key value.</param>
        public void CallOnDeath(Entity died, Entity killer, Quest quest, QuestManager questManager, string baseKey) =>
            OnDeath(died, killer, quest, questManager, baseKey + CachedIdString);
        

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is called when an entity dies.
        /// </summary>
        /// <param name="died">The entity that died.</param>
        /// <param name="killer">The entities killer.</param>
        /// <param name="quest">The quest.</param>
        /// <param name="questManager">The quest manager.</param>
        /// <param name="baseKey">The base key value.</param>
        protected virtual void OnDeath(Entity died, Entity killer, Quest quest, QuestManager questManager,
            string baseKey) { }

        /// <summary>
        /// This method is used to clear all of the tasks progress.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        protected abstract void ClearProgress(QuestManager manager, string baseKey);

        /// <summary>
        /// This method is used to get the number of completed actions.
        /// </summary>
        /// <param name="manager">The quest manager.</param>
        /// <param name="baseKey">The base key.</param>
        /// <returns>The number of completed actions.</returns>
        protected abstract int GetCompletedActions(QuestManager manager, string baseKey);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}