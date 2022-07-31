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
using FishNet.Object;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Amilious.FishNetRpg.Entities;
using FishNet.Object.Synchronizing;
using Amilious.FishNetRpg.Modifiers;

namespace Amilious.FishNetRpg.Statistics {
    
    /// <summary>
    /// This class is used to manage an entities stats.
    /// </summary>
    [RequireComponent(typeof(Entity),typeof(ModifierManager))]
    [AddComponentMenu(FishNetRpg.COMPONENT_MANAGERS+"Stat Manager")]
    public class StatManager : NetworkBehaviour, ISystemManager {

        #region Delagates //////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This delegate is used for the <see cref="StatManager.OnStatInitialized"/> event.
        /// <param name="entity">The entity whose stat was initialized.</param>
        /// <param name="stat">The stat that was initialized</param>
        /// </summary>
        public delegate void OnStatInitializedDelegate(Entity entity, Stat stat);
        
        /// <summary>
        /// This delegate is used for the <see cref="StatManager.OnStatValueUpdated"/> event.
        /// <param name="entity">The entity whose stat was updated.</param>
        /// <param name="stat">The stat that was updated.</param>
        /// </summary>
        public delegate void OnStatValueUpdatedDelegate(Entity entity, Stat stat);
        
        /// <summary>
        /// This delegate is used for the <see cref="StatManager.OnStatManagerInitialized"/> event.
        /// <param name="entity">The entity whose stat manager was initialized.</param>
        /// <param name="systemManager">The manager that was initialized.</param>
        /// </summary>
        public delegate void OnStatManagerInitializedDelegate(Entity entity, StatManager systemManager);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This event is triggered when a stat is initialized.
        /// </summary>
        public static event OnStatInitializedDelegate OnStatInitialized;
        
        /// <summary>
        /// This event is triggered when a stat is updated.
        /// </summary>
        public static event OnStatValueUpdatedDelegate OnStatValueUpdated;
        
        /// <summary>
        /// This event is triggered when a stat manager is initialized.
        /// </summary>
        public static event OnStatManagerInitializedDelegate OnStatManagerInitialized;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Inspector Values ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This list should contains the stats that will be initialized when the manager is loaded.
        /// </summary>
        [SerializeField,Tooltip("The stats that you want your game to have.")] 
        private List<Stat> stats = new();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Sync Objects ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This sync object is used to sync stat values over the network.
        /// </summary>
        [SyncObject] private readonly SyncDictionary<string, StatData> _statData = new ();
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private Instance Variables /////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary is used to store the stat controllers.
        /// </summary>
        private readonly Dictionary<string, StatController> _statInfo = new ();
        
        /// <summary>
        /// This list is used to store the stat modifiers that are active and have a duration.
        /// </summary>
        private readonly List<ModifierSource<IStatModifier>> _durationModifiers = new ();
        
        /// <summary>
        /// This variable is used to cache the entity associated with this stat manager.
        /// </summary>
        private Entity _entity;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property is used to get the <see cref="StatController"/> associated with the given stat name
        /// if it exists, otherwise this property will return null.
        /// </summary>
        /// <param name="statName">The name of the stat.</param>
        public StatController this[string statName] => _statInfo.TryGetValue(statName, out var stat) ? stat : null;

        /// <summary>
        /// This property is used to get the <see cref="StatController"/> associated with the given stat if it
        /// exists, otherwise it will be initialized and returned by this property.
        /// </summary>
        /// <param name="stat">The stat.</param>
        public StatController this[Stat stat] {
            get {
                if(_statInfo.ContainsKey(stat.StatName)) InitializeStat(stat);
                return _statInfo[stat.StatName];
            }
        }
        
        /// <inheritdoc />
        public Entity Entity {
            get {
                //get a reference to the entity if it does not exist.
                _entity ??= GetComponent<Entity>();
                return _entity; //return the entities reference.
            }
        }
        
        /// <summary>
        /// This property is true if the stat manager has been initialized, otherwise false.
        /// </summary>
        public bool Initialized { get; private set; }
        
        /// <inheritdoc />
        public Systems System => Systems.StatsSystem;
        
        /// <inheritdoc />
        public Type SystemType => GetType();

        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Unity Methods //////////////////////////////////////////////////////////////////////////////////////////
             
        /// <summary>
        /// Use the awake method to initialize the stat manager.
        /// </summary>
        private void Awake() => Initialize();             
        
        /// <summary>
        /// This method is used to remove duration modifiers after they expire.
        /// </summary>
        private void FixedUpdate() {
            if(!IsServer||_durationModifiers.Count==0) return;
            //check if modifiers have expired
            var currentTime = Time.realtimeSinceStartup;
            foreach(var modSource in _durationModifiers) {
                if(currentTime - modSource.AppliedTime >= modSource.Modifier.Duration) {
                    _statInfo[modSource.Modifier.StatName]?.RemoveModifier(modSource);
                }
            }
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods /////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a stat to the stat manager.
        /// </summary>
        /// <param name="stat">The stat that you want to add to the stat manager.</param>
        /// <returns>True if the stat was added, otherwise false if the stat already existed.</returns>
        public bool AddStat(Stat stat) {
            if(_statInfo.ContainsKey(stat.StatName)) return false;
            InitializeStat(stat);
            return true;
        }
        
        /// <inheritdoc />
        [Server]
        public bool ApplyModifier(Object source, IModifier modifier) {
            //make sure that the manager is initialized
            Initialize();
            if(modifier is not IStatModifier statModifier) return false;
            if(!_statInfo.TryGetValue(statModifier.StatName, out var stat)) {
                Debug.LogWarningFormat(FishNetRpg.MISSING_STAT,Entity.name,statModifier.StatName);
                return false;
            }
            stat.AddModifier(new ModifierSource<IStatModifier>(statModifier,source));
            return true;
        }
        
        /// <inheritdoc />
        [Server]
        public bool ApplyModifier(int sourceId, IModifier modifier) {
            Initialize();
            if(modifier is not IStatModifier statModifier) return false;
            if(!_statInfo.TryGetValue(statModifier.StatName, out var stat)) {
                Debug.LogWarningFormat(FishNetRpg.MISSING_STAT,Entity.name,statModifier.StatName);
                return false;
            }
            stat.AddModifier(new ModifierSource<IStatModifier>(statModifier,sourceId));
            return true;
        }

        /// <inheritdoc />
        [Server]
        public void RemoveModifiersFromSource(Object source) {
            Initialize();
            foreach(var stat in _statInfo.Values) 
                stat.RemoveModifierFromSource(source);
        }

        /// <inheritdoc />
        [Server]
        public void RemoveModifiersFromSource(int sourceId) {
            Initialize();
            foreach(var stat in _statInfo.Values) 
                stat.RemoveModifierFromSource(sourceId);
        }

        /// <inheritdoc />
        [Server]
        public bool RemoveModifier(Object source, IModifier modifier) {
            Initialize();
            if(modifier is not IStatModifier statModifier) return false;
            if(this[statModifier.Stat] == null) return false;
            return this[statModifier.Stat].RemoveModifier(source,statModifier);
        }

        /// <inheritdoc />
        [Server]
        public bool RemoveModifier(int sourceId, IModifier modifier) {
            Initialize();
            if(modifier is not IStatModifier statModifier) return false;
            if(this[statModifier.Stat] == null) return false;
            return this[statModifier.Stat].RemoveModifier(sourceId,statModifier);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
        #region Private & Protected Methods ////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to initialize the stat manager.
        /// </summary>
        private void Initialize() {
            if(Initialized) return;
            Initialized = true;
            foreach(var stat in stats) InitializeStat(stat);
            Entity.RegisterManager(this); //this is just to make sure
            OnStatManagerInitialized?.Invoke(Entity,this);
        }

        /// <summary>
        /// This method is used to initialize the given stat.
        /// </summary>
        /// <param name="stat">The stat that needs to be initialized.</param>
        protected virtual void InitializeStat(Stat stat) {
            if(_statInfo.ContainsKey(stat.StatName)) {
                Debug.LogWarningFormat(FishNetRpg.EXISTING_STAT,Entity.name,stat.StatName);
                return;
            }
            if(IsServer) {
                var baseValue = stat.BaseValueProvider.BaseValue(1);
                var statData = new StatData {
                    Level = 1,
                    BaseValue = baseValue,
                    Value = baseValue
                };
                _statData[stat.StatName] = statData;
            }
            //add stat to the stat dictionary
            _statInfo[stat.StatName] = new StatController(this, stat,_statData,EventTrigger,WatchDurationModifer);
        }

        /// <summary>
        /// This method is called from a stat controller when an event needs to be triggered.
        /// </summary>
        /// <param name="stat">The stat triggering the event.</param>
        /// <param name="eventToTrigger">The event that is being triggered.</param>
        protected void EventTrigger(Stat stat, StatEventTrigger eventToTrigger) {
            switch(eventToTrigger) {
                case StatEventTrigger.Initialized:OnStatInitialized?.Invoke(Entity,stat); break;
                case StatEventTrigger.Updated:OnStatValueUpdated?.Invoke(Entity,stat); break;
                default: return;
            }
        }

        /// <summary>
        /// This method is called by a stat controller when a duration modifier is applied.
        /// </summary>
        /// <param name="source">The modifier source.</param>
        protected void WatchDurationModifer(ModifierSource<IStatModifier> source) => _durationModifiers.Add(source);
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }

    /// <summary>
    /// This enum is used to trigger a stats event.
    /// </summary>
    public enum StatEventTrigger{Initialized,Updated}
    
}