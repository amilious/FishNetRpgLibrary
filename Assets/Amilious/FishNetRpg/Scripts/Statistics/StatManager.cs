using System;
using UnityEngine;
using FishNet.Object;
using System.Collections.Generic;
using Amilious.FishNetRpg.Entities;
using FishNet.Object.Synchronizing;
using Object = UnityEngine.Object;

namespace Amilious.FishNetRpg.Statistics {
    
    [RequireComponent(typeof(Entity))]
    public class StatManager : NetworkBehaviour {

        public delegate void OnStatInitializedDelegate(Entity entity, Stat stat);
        public delegate void OnStatValueUpdatedDelegate(Entity entity, Stat stat);
        public delegate void OnStatManagerInitializedDelegate(Entity entity, StatManager manager);

        public static event OnStatInitializedDelegate OnStatInitialized;
        public static event OnStatValueUpdatedDelegate OnStatValueUpdated;
        public static event OnStatManagerInitializedDelegate OnStatManagerInitialized;
        
        [SerializeField] private Stat health;
        [SerializeField] private List<Stat> otherStats = new List<Stat>();

        [SyncObject]
        private readonly SyncDictionary<string, StatData> _statData = new SyncDictionary<string, StatData>();
        private readonly Dictionary<string, StatController> _statInfo = new Dictionary<string, StatController>();
        private readonly List<StatModifierSource> _durationModifiers = new List<StatModifierSource>();
        private Entity _entity;
        /// <summary>
        /// This constant contains the message that will be displayed for a missing stat.
        /// </summary>
        private const string MISSING_STAT =
            "The StatManager of the type \"{0}\" does not contain a stat with the name \"{1}\"!";
        
        public StatController this[string statName] => _statInfo.TryGetValue(statName, out var stat) ? stat : null;

        public StatController this[Stat stat] {
            get {
                if(_statInfo.ContainsKey(stat.StatName)) InitializeStat(stat);
                return _statInfo[stat.StatName];
            }
        }
        
        public bool AddStat(Stat stat) {
            if(_statInfo.ContainsKey(stat.StatName)) return false;
            InitializeStat(stat);
            return true;
        }

        public Entity Entity {
            get {
                _entity ??= GetComponent<Entity>();
                return _entity;
            }
        }

        private void Awake() {
            Initialize();
        }

        public bool Initialized { get; private set; }
        
        private void Initialize() {
            if(Initialized) return;
            if(health == null) {
                Debug.LogWarning("The StatManager must have a stat for health!");
                return;
            }
            Initialized = true;
            InitializeStat(health);
            foreach(var stat in otherStats) InitializeStat(stat);
            OnStatManagerInitialized?.Invoke(Entity,this);
        }

        protected virtual void InitializeStat(Stat stat) {
            if(_statInfo.ContainsKey(stat.StatName)) {
                Debug.LogWarningFormat("The StatManager already contains a stat with the name {0}!",stat.StatName);
                return;
            }
            if(IsServer) {
                //TODO: load the values if they are saved only the level needs to be loaded
                var baseValue = stat.BaseValueProvider.BaseValue(1);
                var statData = new StatData() {
                    Level = 1,
                    BaseValue = baseValue,
                    Value = baseValue
                };
                _statData[stat.StatName] = statData;
            }
            //add stat to the stat dictionary
            _statInfo[stat.StatName] = new StatController(this, stat,_statData,EventTrigger,WatchDurationModifer);
        }
        
        private void Update() {
            if(!IsServer||_durationModifiers.Count==0) return;
            //check if modifiers have expired
            var currentTime = Time.realtimeSinceStartup;
            foreach(var modSource in _durationModifiers) {
                if(currentTime - modSource.AppliedTime >= modSource.Modifier.Duration) {
                    _statInfo[modSource.Modifier.StatName]?.RemoveModifier(modSource);
                }
            }
        }

        [Server]
        public bool ApplyModifier(Object source, IStatModifier modifier) {
            //make sure that the manager is initialized
            Initialize();
            if(!_statInfo.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(MISSING_STAT,GetType().Name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new StatModifierSource(modifier,source));
            return true;
        }
        
        [Server]
        public bool ApplyModifier(int sourceId, IStatModifier modifier) {
            Initialize();
            if(!_statInfo.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(MISSING_STAT,GetType().Name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new StatModifierSource(modifier,sourceId));
            return true;
        }

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
        

        protected void EventTrigger(Stat stat, StatEventTrigger eventToTrigger) {
            switch(eventToTrigger) {
                case StatEventTrigger.Initialized:OnStatInitialized?.Invoke(Entity,stat); break;
                case StatEventTrigger.Updated:OnStatValueUpdated?.Invoke(Entity,stat); break;
                default: throw new ArgumentOutOfRangeException(nameof(eventToTrigger), eventToTrigger, null);
            }
        }

        protected void WatchDurationModifer(StatModifierSource source) => _durationModifiers.Add(source);

    }
    
    
    public enum StatEventTrigger{Initialized,Updated}
    
}