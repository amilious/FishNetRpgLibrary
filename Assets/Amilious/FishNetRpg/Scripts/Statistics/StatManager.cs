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
        
        /// <summary>
        /// This property is used to get or return a cached reference to the <see cref="Entity"/> to whom this
        /// manager belongs to.
        /// </summary>
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
        
        /// <summary>
        /// This method is used to add a modifier to a stat.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="source">The source that is applying the modifier.</param>
        /// <param name="modifier">The modifier that you want to apply.</param>
        /// <returns>True if able to apply the modifier, otherwise false.</returns>
        [Server]
        public bool ApplyModifier(Object source, IStatModifier modifier) {
            //make sure that the manager is initialized
            Initialize();
            if(!_statInfo.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(FishNetRpg.MISSING_STAT,Entity.name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new ModifierSource<IStatModifier>(modifier,source));
            return true;
        }
        
        /// <summary>
        /// This method is used to add a modifier to a stat.  This method should only be
        /// called by the server.
        /// </summary>
        /// <param name="sourceId">The source that is applying the modifier.</param>
        /// <param name="modifier">The modifier that you want to apply.</param>
        /// <returns>True if able to apply the modifier, otherwise false.</returns>
        [Server]
        public bool ApplyModifier(int sourceId, IStatModifier modifier) {
            Initialize();
            if(!_statInfo.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(FishNetRpg.MISSING_STAT,Entity.name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new ModifierSource<IStatModifier>(modifier,sourceId));
            return true;
        }

        /// <summary>
        /// This method is used to remove all the modifiers that were assigned by the given source.
        /// </summary>
        /// <param name="source">The source.</param>
        [Server]
        public void RemoveModifiersFromSource(Object source) {
            Initialize();
            foreach(var stat in _statInfo.Values) 
                stat.RemoveModifierFromSource(source);
        }

        /// <summary>
        /// This method is used to remove all modifiers of the given type.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers that you want to remove.</param>
        [Server]
        public void RemoveModifiersFromSource(int sourceId) {
            Initialize();
            foreach(var stat in _statInfo.Values) 
                stat.RemoveModifierFromSource(sourceId);
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