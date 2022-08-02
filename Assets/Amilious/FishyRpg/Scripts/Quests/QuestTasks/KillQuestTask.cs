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
using System.Text;
using UnityEngine;
using Amilious.Core.Attributes;
using System.Collections.Generic;
using Amilious.FishyRpg.Entities;
using Amilious.FishyRpg.Requirements;

namespace Amilious.FishyRpg.Quests.QuestTasks {
    
    /// <summary>
    /// This class is used as a kill task for a quest.
    /// </summary>
    [CreateAssetMenu(fileName = "NewKillTask", 
    menuName = FishyRpg.QUEST_MENU_ROOT+"Tasks/New Kill Task", order = FishyRpg.QUEST_START+100)]
    public class KillQuestTask : QuestTask<Entity,Entity> {

        #region Constants //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This constant is used as the key for storing the kill count.
        /// </summary>
        private const string KILLS = "kills";
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Serialized Fields //////////////////////////////////////////////////////////////////////////////////////

        [SerializeField, Tooltip("If true you can use an entity group instead of an entity type.")] 
        private bool useEntityGroup;
        [SerializeField, Tooltip("If true party members kill's will be counted for the quest.")] 
        private bool countPartyKills = true;
        [SerializeField, ShowIf(nameof(countPartyKills)), Tooltip("If true party followers kill's will be counted for the quest.")] 
        private bool countFollowerKills = true;
        [SerializeField, ShowIf(nameof(useEntityGroup)),Tooltip("The entity group that will count as kills.")]
        private EntityGroup entityGroup;
        [SerializeField, HideIf(nameof(useEntityGroup)),Tooltip("The entity group that will count as kills.")]
        private EntityType entityType;
        [SerializeField, Tooltip("The number of entities to kill.")] 
        private int entitiesToKill;
        [SerializeField, Tooltip("Requirements that the killed entities must meet to be counted.")]
        private List<AbstractRequirement> killedRequirements = new();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override int TotalActions => useEntityGroup ? entityGroup == null ? 0 : entitiesToKill :
            entityType == null ? 0 : entitiesToKill;

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void ClearProgress(QuestManager manager, Quest quest, StringBuilder baseKey) {
            manager.ClearData(baseKey.Append(KILLS));
        }

        /// <inheritdoc />
        protected override int GetCompletedActions(QuestManager manager, Quest quest, StringBuilder baseKey) {
            return manager[baseKey.Append(KILLS)];
        }
        
        /// <inheritdoc />
        protected override void Callback(QuestManager manager, Quest quest, StringBuilder baseKey, 
            Entity died, Entity killer) {
            if(!countPartyKills&&killer.ObjectId!=manager.Player.ObjectId) return;
            if(countFollowerKills && !killer.IsPlayerOrParty(manager.Player, countFollowerKills)) return;
            if(!killedRequirements.All(x => x.MeetsRequirement(died))) return;
            manager[baseKey.Append(KILLS)]++;
            manager.QuestUpdated(quest);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Use on enable to subscribe to the death listener.
        /// </summary>
        private void OnEnable() => Entity.OnEntityDied += OnEntityDied;

        /// <summary>
        /// Use on disable to unsubscribe to the death listener.
        /// </summary>
        private void OnDisable() => Entity.OnEntityDied -= OnEntityDied;

        /// <summary>
        /// This method is triggered when an entity dies.
        /// </summary>
        /// <param name="died">The entity that died.</param>
        /// <param name="killer">The entity that killed the dead entity.</param>
        /// <param name="_">The death message.</param>
        private void OnEntityDied(Entity died, Entity killer, string _) {
            if(ActiveManagers.Count == 0) return; 
            //make sure that killed is not null
            if(died == null||killer==null) return;
            //check to make sure that it is for a player that matters
            var send = false;
            foreach(var player in Players) {
                if(player==null) continue;
                if(countPartyKills) {
                    if(!killer.IsPlayerOrParty(player, countFollowerKills)) continue;
                    send = true;
                    break;
                }
                if(killer.ObjectId != player.ObjectId) continue;
                send = true;
                break;
            }
            //update the quests.
            if(send)SendData(this,died,killer);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}