using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using FishNet.Object.Synchronizing.Internal;
using Object = UnityEngine.Object;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This class is used to represent a stat.
    /// </summary>
    [Serializable]
    public class Stat : SyncBase {

        [SerializeField] private int _baseValue;
        [SerializeField] private int _value;
        [SerializeField] private int _cap;
        [SerializeField] private int _minimumValue;
        private List<StatModifier> _modifiers = new List<StatModifier>();

        public int BaseValue {
            get { return _baseValue; }
            set {
                if(value == _baseValue) return;
                _baseValue = value;
                CalculateValue();
                Dirty();
            }
        }

        public int Value {
            get { return _value; }
            protected set {
                if(value == _value) return;
                _value = value;
                Dirty();
            }
        }

        public int Cap {
            get => _cap;
            set {
                if(value == _cap) return;
                _cap = value;
                CalculateValue();
                Dirty();
            }
        }

        public int Minimum {
            get => _minimumValue;
            set {
                if(value == _minimumValue) return;
                _minimumValue = value;
                CalculateValue();
                Dirty();
            }
        }

        public void AddModifier(StatModifier modifier) {
            _modifiers.Add(modifier);
            CalculateValue();
        }

        public void RemoveModifierFromSource(Object source) {
            _modifiers = _modifiers.Where(modifier => modifier.HasSource(source)).ToList();
            CalculateValue();
        }

        public void RemoveModifierFromSource(int sourceId) {
            _modifiers = _modifiers.Where(modifier => modifier.HasSource(sourceId)).ToList();
            CalculateValue();
        }
        
        protected void CalculateValue() {
            //start with the base value
            int newValue = BaseValue;
            //sort the modifiers so that they are applied in the correct order.
            _modifiers.Sort(SortModifiers);
            //apply all of the modifiers
            newValue = _modifiers.Aggregate(newValue, (current, modifier) => modifier.ApplyModifier(current));
            //make sure that the value does not exceed the cap
            if(Cap >= 0) newValue = Mathf.Min(newValue, Cap);
            //set the new value
            Value = newValue;
        }

        protected virtual int SortModifiers(StatModifier a, StatModifier b) {
            return a.Operation.CompareTo(b.Operation);
        }

    }
    
}