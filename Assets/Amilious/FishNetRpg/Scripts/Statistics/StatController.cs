using System;
using FishNet;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using Object = UnityEngine.Object;

namespace Amilious.FishNetRpg.Statistics {
    
    /// <summary>
    /// This class is used to preform actions on a stat.
    /// </summary>
    public class StatController {
        
        #region Instance Variables /////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This list is used to hold the modifiers that are currently applied to this stat.
        /// </summary>
        private List<StatModifierSource> _modifierSources = new List<StatModifierSource>();

        /// <summary>
        /// This bool is used to keep track of if the modifiers are sorted.
        /// </summary>
        private bool _sorted;
        
        #endregion

        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        public bool Initialized { get; private set; }

        public StatManager StatManager { get; }
        
        public Stat Stat { get; }
        
        public string StatName { get; }

        public int Level {
            get => StatData.Level;
            set {
                if(!IsServer()) return;
                if(StatData.Level == value) return;
                StatData.Level = value;
                StatData.BaseValue = Stat.BaseValueProvider.BaseValue(value);
                CalculateValue();
            }
        }

        public int Cap => Stat.BaseValueProvider.GetCap(Level);

        public int Minimum => Stat.BaseValueProvider.GetMinimum(Level);

        public int Value => StatData.Value;

        public int BaseValue => StatData.BaseValue;

        protected StatData StatData => StatDataDictionary.TryGetValue(StatName, out var statData) ? statData : 
            new StatData(1,Stat.BaseValueProvider.BaseValue(1),Stat.BaseValueProvider.BaseValue(1));

        protected SyncDictionary<string, StatData> StatDataDictionary { get; }
        
        protected Action<Stat, StatEventTrigger> EventTrigger { get; } 
        
        protected Action<StatModifierSource> WatchDurationModifer { get; }
        

        #endregion
        
        
        #region Constructors /////////////////////////////////////////////////////////////////////////////////////////// 
        
        public StatController(StatManager manager, Stat stat, SyncDictionary<string, StatData> statData, 
            Action<Stat, StatEventTrigger> eventTrigger, Action<StatModifierSource> watchDurationModifer) {
            StatManager = manager;
            Stat = stat;
            StatName = stat.StatName;
            StatDataDictionary = statData;
            EventTrigger = eventTrigger;
            WatchDurationModifer = watchDurationModifer;
            StatDataDictionary.OnChange += StatDataChanged;
            Initialize();
        }

        #endregion
        
        
        #region Modifier Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a modifier to the stat.  This method can only be called on the server.
        /// </summary>
        /// <param name="source">The source with the modifier that you want to add.</param>
        public void AddModifier(StatModifierSource source) {
            if(!IsServer()) return;
            _sorted = false;
            //only allow a single override modifier
            if(source.Modifier.ModifierType == ModifierType.Override) {
                for(var i =0; i < _modifierSources.Count;i++)
                    if(_modifierSources[i].Modifier.ModifierType == ModifierType.Additive) {
                        _modifierSources[i] = source;
                        CalculateValue();
                    }
            }
            _modifierSources.Add(source);
            if(source.Modifier.Duration >= 0) WatchDurationModifer.Invoke(source);
            CalculateValue();
        }

        /// <summary>
        /// This method is used to remove modifiers with the given source.  This method can only be called on
        /// the server.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        public void RemoveModifierFromSource(Object source) {
            if(!IsServer()) return;
            _modifierSources = _modifierSources.Where(modifier => modifier.HasSource(source)).ToList();
            CalculateValue();
        }

        /// <summary>
        /// This method is used to remove modifiers with the given source.  This method can only be called on
        /// the server.
        /// </summary>
        /// <param name="sourceId">The id of the source of the modifier.</param>
        public void RemoveModifierFromSource(int sourceId) {
            if(!IsServer()) return;
            _modifierSources = _modifierSources.Where(modifier => modifier.HasSource(sourceId)).ToList();
            CalculateValue();
        }

        /// <summary>
        /// This method is used to clear all of the stat's modifiers.  This method can only be called on the server.
        /// </summary>
        public void ClearAllModifiers() {
            _modifierSources.Clear();
            CalculateValue();
        }

        /// <summary>
        /// This method is used to remove a modifier from the stat.  This method can only be called on the server.
        /// </summary>
        /// <param name="modifierSource"></param>
        public void RemoveModifier(StatModifierSource modifierSource) {
            _modifierSources.Remove(modifierSource);
            CalculateValue();
        }
        
        #endregion
        
        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to calculate the value based on
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected void CalculateValue() {
            //start with the base value
            var newValue = Stat.BaseValueProvider.BaseValue(Level);
            float multiplier = 0;
            var appliedMultiplier = false;
            //sort the modifiers so that they are applied in the correct order.
            if(!_sorted)_modifierSources.Sort(SortModifiers);
            _sorted = true;
            //apply all of the modifiers
            foreach(var source in _modifierSources) {
                var modifier = source.Modifier;
                if(modifier.ModifierType > ModifierType.AdditiveMultiplier && !appliedMultiplier) {
                    if(multiplier!=0) newValue = Mathf.RoundToInt(newValue * multiplier);
                    appliedMultiplier = true;
                }
                switch(modifier.ModifierType) {
                    case ModifierType.Additive: newValue += Mathf.RoundToInt(modifier.Amount); break;
                    case ModifierType.AdditiveMultiplier: multiplier += modifier.Amount; break;
                    case ModifierType.StackableMultiplier: 
                        newValue = Mathf.RoundToInt(newValue * modifier.Amount); break;
                    case ModifierType.PostMultiplierAdditive: newValue += Mathf.RoundToInt(modifier.Amount); break;
                    case ModifierType.Override: newValue = Mathf.RoundToInt(modifier.Amount); break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            //make sure that multipliers are added if they haven't been
            if(!appliedMultiplier&&multiplier!=0) newValue = Mathf.RoundToInt(newValue * multiplier);
            //make sure that the value does not exceed the cap
            if(Cap >= 0) newValue = Mathf.Min(newValue, Cap);
            //make sure that the value is not less than the minimum
            newValue = Mathf.Max(Minimum, newValue);
            //set the new value
            StatData.Value = newValue;
            //mark dirty
            Dirty();
        }

        /// <summary>
        /// This method is used to sort the modifiers.
        /// </summary>
        /// <param name="a">The first modifier to compare.</param>
        /// <param name="b">The second modifier to compare.</param>
        /// <returns>The result of the comparison.</returns>
        protected static int SortModifiers(StatModifierSource a, StatModifierSource b) {
            return a.Modifier.ModifierType.CompareTo(b.Modifier.ModifierType);
        }
        
        /// <summary>
        /// This method is used to check if running on the server.  If not on the server it will debug a warning to
        /// the console.
        /// </summary>
        /// <returns>True if running on the server.</returns>
        protected bool IsServer() {
            if(InstanceFinder.IsServer) return true;
            Debug.LogWarning("Values cannot be changed on the client!");
            return false;
        }

        private void StatDataChanged(SyncDictionaryOperation op, string key, StatData value, bool asServer) {
            if(InstanceFinder.IsHost && asServer) return; //do not duplicate events if running on host.
            Initialize();
            if(key==StatName) EventTrigger.Invoke(Stat, StatEventTrigger.Updated);
        }

        protected void Initialize() {
            if(Initialized) return;
            if(!StatDataDictionary.ContainsKey(StatName)) return;
            Initialized = true;
            EventTrigger.Invoke(Stat, StatEventTrigger.Initialized);
        }

        protected void Dirty() => StatDataDictionary.Dirty(StatName);

        #endregion
        
    }
}