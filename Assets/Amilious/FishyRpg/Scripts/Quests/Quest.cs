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

        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////
        
        [SerializeField] private string questName;
        [SerializeField, Multiline] private string questDescription;
        [SerializeField] private List<QuestTask> questTasks = new();
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
        /// This method is used to 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuestTask TaskById(long id) => questTasks.FirstOrDefault(x => x.Id == id);

        /// <summary>
        /// This method is used to check if the quest is complete.
        /// </summary>
        /// <param name="questManager">The quest manager.</param>
        /// <returns>True if the quest is complete, otherwise false.</returns>
        public bool IsComplete(QuestManager questManager) {
            return questTasks.All(x => x.IsComplete(questManager, CachedIdString));
        }

        /// <summary>
        /// This method is used to clear all quest progress.
        /// </summary>
        /// <param name="questManager">The quest manager.</param>
        public void ClearAllProgress(QuestManager questManager) {
            questTasks.ForEach(x=>x.ClearAllProgress(questManager, CachedIdString));
        }
        
        /// <summary>
        /// This method is used to check if an entity meets all of the requirements for the quest.
        /// </summary>
        /// <param name="player">The player that you are checking for.</param>
        /// <returns>True if the player meets all of the requirements, otherwise false.</returns>
        public bool MeetsAllRequirements(Player player) {
            return requirements.All(requirement => requirement.MeetsRequirement(player));
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
        
        /// <summary>
        /// This method is called when an entity dies.
        /// </summary>
        /// <param name="died">The entity that died.</param>
        /// <param name="killer">The entities killer.</param>
        /// <param name="questManager">The quest manager.</param>
        public void OnDeath(Entity died, Entity killer, QuestManager questManager) {
            questTasks.ForEach(x=>x.CallOnDeath(died,killer,this,questManager,CachedIdString));
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}