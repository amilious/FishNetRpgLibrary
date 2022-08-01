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
using System.Text;
using FishNet.Object;
using Amilious.FishyRpg.Entities;
using FishNet.Object.Synchronizing;
using Amilious.FishyRpg.Quests.QuestTasks;

namespace Amilious.FishyRpg.Quests {
    
    [RequireComponent(typeof(Player))]
    public class QuestManager : AmiliousNetworkBehavior {

        public delegate void QuestUpdateDelegate(Player player, QuestManager manager, Quest quest);

        public static event QuestUpdateDelegate OnQuestTaken;
        public static event QuestUpdateDelegate OnQuestUpdated;
        public static event QuestUpdateDelegate OnQuestCompleted;
        public static event QuestUpdateDelegate OnQuestAbandoned;

        [SyncObject] private readonly SyncDictionary<string, int> _questData = new SyncDictionary<string, int>();
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

        public int this[StringBuilder key] {
            get {
                _questData.TryGetValue(key.ToString(), out var value);
                return value; //this will return 0 if the key is empty
            }
            [Server] set => _questData[key.ToString()] = value;
        }

        public int this[string key, int max, bool arrayCountMax = false] {
            get {
                _questData.TryGetValue(key, out var value);
                return Mathf.Min(value,max-(arrayCountMax?1:0)); //this will return 0 if the key is empty
            }
            [Server] set => _questData[key] = Mathf.Min(value,max-(arrayCountMax?1:0));
        }
        
        public int this[string key] {
            get {
                _questData.TryGetValue(key, out var value);
                return value; //this will return 0 if the key is empty
            }
            [Server] set => _questData[key] = value;
        }

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        public void TriggerCallback<T,TV1>(TV1 val1)
            where T : QuestTask<TV1> {
            foreach(var quest in _activeQuests) quest.TriggerCallback<T,TV1>(this,val1);
        }

        public void TriggerCallback<T,TV1,TV2>(TV1 val1, TV2 val2)
            where T : QuestTask<TV1,TV2> {
            foreach(var quest in _activeQuests) quest.TriggerCallback<T,TV1,TV2>(this,val1, val2);
        }
        
        public void TriggerCallback<T,TV1,TV2,TV3>(TV1 val1, TV2 val2, TV3 val3)
            where T : QuestTask<TV1,TV2,TV3> {
            foreach(var quest in _activeQuests) quest.TriggerCallback<T,TV1,TV2,TV3>(this,val1, val2, val3);
        }
        
        public void TriggerCallback<T, TV1, TV2, TV3, TV4>(TV1 val1, TV2 val2, TV3 val3, TV4 val4)
            where T : QuestTask<TV1,TV2,TV3,TV4> {
            foreach(var quest in _activeQuests) quest.TriggerCallback<T,TV1,TV2,TV3,TV4>(this,val1, val2, val3, val4);
        }
        
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
            quest.AddManager(this);
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
        /// This method is used to clear the quest data.
        /// </summary>
        /// <param name="key">The key that you want to clear.</param>
        public void ClearData(string key) {
            if(_questData.ContainsKey(key)) _questData.Remove(key);
        }

        /// <summary>
        /// This method is used to clear the quest data.
        /// </summary>
        /// <param name="key">The key that you want to clear.</param>
        public void ClearData(StringBuilder key) => ClearData(key.ToString());

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
        
    }
}