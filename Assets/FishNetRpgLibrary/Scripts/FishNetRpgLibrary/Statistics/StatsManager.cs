using System;
using UnityEngine;
using System.Linq;
using FishNet.Object;
using System.Reflection;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using FishNet.Object.Synchronizing;
using FishNetRpgLibrary.Entity;

namespace FishNetRpgLibrary.Statistics {
    
    [RequireComponent(typeof(LivingEntity))]
    public class StatsManager : NetworkBehaviour {

        #region Private Static Variables ///////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This dictionary contains the stat FieldInfo references
        /// </summary>
        private static readonly Dictionary<Type, List<FieldInfo>> FieldCache =
            new Dictionary<Type, List<FieldInfo>>();
        
        #endregion
        
        
        #region Delegates //////////////////////////////////////////////////////////////////////////////////////////////

        public delegate void OnStatValueChangedDelegate(Stat stat);
        
        #endregion
        
        
        #region Events /////////////////////////////////////////////////////////////////////////////////////////////////
                       
        
        #endregion
        
        
        #region Private Instance Variables /////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This variable keeps track if the manager has been initialized.
        /// </summary>
        private bool _initialized;
        
        /// <summary>
        /// This constant contains the message that will be displayed for a missing stat.
        /// </summary>
        private const string MISSING_STAT =
            "The StatManager of the type \"{0}\" does not contain a stat with the name \"{1}\"!";

        /// <summary>
        /// This dictionary contains the stat references.
        /// </summary>
        private readonly Dictionary<string, Stat> _statDictionary = new Dictionary<string, Stat>();

        /// <summary>
        /// This list contains the modifiers that need to be removed after a duration.
        /// </summary>
        private readonly List<StatModifierSource> _durationModifiers = new List<StatModifierSource>();

        #endregion
        
        public LivingEntity Entity { get; private set; }

        public Stat this[string statName] {
            get {
                //make sure that the manager is initialized
                Initialize();
                return _statDictionary.TryGetValue(statName, out var stat) ? stat : null;
            }
        }
        
        [SyncObject]
        private readonly Stat _defence = new Stat("Defence");
        
        private void Awake() {
            Entity = GetComponent<LivingEntity>();
            Initialize();
        }

        protected virtual void Initialize() {
            if(_initialized) return;
            _initialized = true;
            var type = GetType();
            //build the field cached for this class type if it does not exist
            if(!FieldCache.ContainsKey(type)) {
                //create dictionary
                //get the stat types
                var stats = GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.FieldType.IsAssignableFrom(typeof(Stat)));
                var fieldList = stats.ToList();
                FieldCache[type] = fieldList;
            }
            //crate the stat dictionary for this class.
            foreach(var statInfo in FieldCache[type]) {
                var stat = (Stat)statInfo.GetValue(this);
                if(stat==null) continue;
                if(_statDictionary.ContainsKey(stat.Name)) {
                    Debug.LogWarningFormat("The stat manager has multiple stats with the name {0}!", stat.Name);
                    continue;
                }
                _statDictionary[stat.Name] = stat;
                _statDictionary[stat.Name]?.SetStatsManager(this);
            }
        }

        private void Update() {
            if(!IsServer||_durationModifiers.Count==0) return;
            //check if modifiers have expired
            var currentTime = Time.realtimeSinceStartup;
            foreach(var modSource in _durationModifiers) {
                if(currentTime - modSource.AppliedTime >= modSource.Modifier.Duration) {
                    _statDictionary[modSource.Modifier.StatName]?.RemoveModifier(modSource);
                }
            }
        }

        [Server]
        public bool ApplyModifier(Object source, IStatModifier modifier) {
            //make sure that the manager is initialized
            Initialize();
            if(!_statDictionary.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(MISSING_STAT,GetType().Name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new StatModifierSource(modifier,source));
            return true;
        }
        
        [Server]
        public bool ApplyModifier(int sourceId, IStatModifier modifier) {
            Initialize();
            if(!_statDictionary.TryGetValue(modifier.StatName, out var stat)) {
                Debug.LogWarningFormat(MISSING_STAT,GetType().Name,modifier.StatName);
                return false;
            }
            stat.AddModifier(new StatModifierSource(modifier,sourceId));
            return true;
        }

        [Server]
        public void RemoveModifiersFromSource(Object source) {
            Initialize();
            foreach(var stat in _statDictionary.Values) 
                stat.RemoveModifierFromSource(source);
        }

        /// <summary>
        /// This method is used to remove all modifiers of the given type.
        /// </summary>
        /// <param name="sourceId">The source of the modifiers that you want to remove.</param>
        [Server]
        public void RemoveModifiersFromSource(int sourceId) {
            Initialize();
            foreach(var stat in _statDictionary.Values) 
                stat.RemoveModifierFromSource(sourceId);
        }

        [Server]
        public void WatchDuration(StatModifierSource source) => _durationModifiers.Add(source);

    }

}
