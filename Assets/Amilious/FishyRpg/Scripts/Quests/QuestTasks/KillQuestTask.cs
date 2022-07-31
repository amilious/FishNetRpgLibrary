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
using Amilious.Core.Attributes;
using Amilious.FishyRpg.Entities;

namespace Amilious.FishyRpg.Quests.QuestTasks {
    
    /// <summary>
    /// This class is used as a kill task for a quest.
    /// </summary>
    public class KillQuestTask : QuestTask {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        private const string KILLED = "killed";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, Tooltip("If true you can use an entity group instead of an entity type.")] 
        private bool useEntityGroup;
        [SerializeField, Tooltip("If true party members kill's will be counted for the quest.")] 
        private bool countPartyKills = true;
        [SerializeField, ShowIf(nameof(useEntityGroup))]
        private EntityGroup entityGroup;
        [SerializeField, HideIf(nameof(useEntityGroup))]
        private EntityType entityType;
        [SerializeField] private int entitiesToKill;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override int TotalActions => useEntityGroup ? entityGroup == null ? 0 : entitiesToKill :
            entityType == null ? 0 : entitiesToKill;

        /// <inheritdoc />
        protected override void OnDeath(Entity died, Entity killer, Quest quest, QuestManager questManager, string baseKey) {
            if(killer != questManager.Player &&
               (!countPartyKills || !questManager.Player.Party.Contains(killer, true))) return;
            questManager.SetQuestData(baseKey + KILLED, GetCompletedActions(questManager, baseKey) + 1);
            questManager.QuestUpdated(quest);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void ClearProgress(QuestManager manager, string baseKey) {
            manager.ClearQuestData(baseKey+KILLED);
        }

        /// <inheritdoc />
        protected override int GetCompletedActions(QuestManager manager, string baseKey) {
            manager.TryGetQuestData<int>(baseKey + KILLED, out var kills);
            return kills;
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}