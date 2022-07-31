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
using UnityEngine;
using System.Collections.Generic;
using Amilious.FishyRpg.Entities;
using Amilious.FishyRpg.Extensions;
using FishNet.Object.Synchronizing;

namespace Amilious.FishyRpg.Quests {
    
    [RequireComponent(typeof(Player))]
    public class QuestManager : AmiliousNetworkBehavior {

        public delegate void QuestUpdateDelegate(Player player, QuestManager manager, Quest quest);

        public static event QuestUpdateDelegate OnQuestTaken;
        public static event QuestUpdateDelegate OnQuestUpdated;
        public static event QuestUpdateDelegate OnQuestCompleted;
        public static event QuestUpdateDelegate OnQuestAbandoned;

        [SyncObject] private readonly SyncDictionary<string, object> _questData = new SyncDictionary<string, object>();
        [SyncObject] private readonly SyncList<Quest> _activeQuests = new SyncList<Quest>();
        [SyncObject] private readonly SyncList<Quest> _completedQuests = new SyncList<Quest>();

        private Player _player;

        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This property contains the player that this quest manager is for.
        /// </summary>
        public Player Player => _player ??= GetComponent<Player>();

        /// <summary>
        /// This property is used to get the status of the given <see cref="Quest"/>.
        /// </summary>
        /// <param name="quest">The quest that you want to get the status for.</param>
        public QuestStatus this[Quest quest] {
            get {
                if(_completedQuests.Contains(quest)) return QuestStatus.Completed;
                return _activeQuests.Contains(quest) ? QuestStatus.Active : QuestStatus.NotStarted;
            }
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to try add a quest.
        /// </summary>
        /// <param name="quest">The quest that you want to add.</param>
        /// <returns>True if the quest was added, otherwise false.</returns>
        public bool TryAddQuest(Quest quest) {
            if(_completedQuests.Contains(quest)) return false;
            if(_activeQuests.Contains(quest)) return false;
            if(quest.MeetsAllRequirements(Player)) return false;
            _activeQuests.Add(quest);
            quest.OnQuestTaken(this);
            OnQuestTaken?.Invoke(Player,this,quest);
            return true;
        }

        /// <summary>
        /// This method is used to try abandon a quest.
        /// </summary>
        /// <param name="quest">The quest that you want to abandon.</param>
        /// <returns>True if the quest was active and was abandoned, otherwise false.</returns>
        public bool TryAbandonQuest(Quest quest) {
            if(_completedQuests.Contains(quest)) return false;
            if(!_activeQuests.Contains(quest)) return false;
            if(!quest.CanAbandon) return false;
            _activeQuests.Remove(quest);
            quest.OnQuestAbandoned(this);
            OnQuestAbandoned?.Invoke(Player, this, quest);
            quest.ClearAllProgress(this);
            return true;
        }

        /// <summary>
        /// This method is used to try get quest data.
        /// </summary>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value for the given key.</param>
        /// <typeparam name="T">The type of data.</typeparam>
        /// <returns>True if the data was found and was the give type, otherwise false.</returns>
        public bool TryGetQuestData<T>(string key, out T value) {
            return _questData.TryGetCastValueIL2CPP(key, out value);
        }

        /// <summary>
        /// This method is used to set data.
        /// </summary>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value.</param>
        public void SetQuestData(string key, object value) {
            _questData[key] = value;
        }

        /// <summary>
        /// This method is used to try add data.
        /// </summary>
        /// <param name="key">The key for the data.</param>
        /// <param name="value">The value of the data.</param>
        /// <returns>True if the data was added, otherwise false if the key was taken.</returns>
        public bool TryAddQuestData<T>(string key, object value) {
            return _questData.TryAdd(key, value);
        }

        /// <summary>
        /// This method is used to clear the quest data.
        /// </summary>
        /// <param name="key">The key that you want to clear.</param>
        public void ClearQuestData(string key) {
            if(_questData.ContainsKey(key)) _questData.Remove(key);
        }

        /// <summary>
        /// This method should be called whenever data has been updated for a quest.
        /// </summary>
        /// <param name="quest">The quest that was updated.</param>
        public void QuestUpdated(Quest quest) {
            if(!_activeQuests.Contains(quest)) return;
            if(_completedQuests.Contains(quest)) return;
            //trigger update
            quest.OnQuestUpdated(this);
            OnQuestUpdated?.Invoke(Player,this,quest);
            //check if complete
            if(!quest.IsComplete(this)) return;
            _activeQuests.Remove(quest);
            _completedQuests.Add(quest);
            quest.OnQuestComplete(this);
            OnQuestCompleted?.Invoke(Player, this, quest);
            //all the quest items have been completed so we can clear the quest data.
            quest.ClearAllProgress(this);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Methods ////////////////////////////////////////////////////////////////////////////////////////

        private void OnEnable() {
            Entity.OnEntityDied += OnEntityDied;
        }

        private void OnDisable() {
            Entity.OnEntityDied -= OnEntityDied;
        }

        private void OnEntityDied(Entity killed, Entity killer, string _) {
            foreach(var quest in _activeQuests) quest.OnDeath(killed,killer,this);
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}