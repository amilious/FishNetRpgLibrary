using System;
using System.Linq;
using UnityEngine;
using FishNet.Serializing;
using System.Collections.Generic;
using FishNet.Object.Synchronizing;
using Object = UnityEngine.Object;
using FishNet.Object.Synchronizing.Internal;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This class is used to represent a stat.
    /// </summary>
    public class Stat : SyncBase, ICustomSync {

        
        #region Private Synced Instance Variables //////////////////////////////////////////////////////////////////////
        
        private int _baseValue, _value, _cap, _minimumValue;
        private bool _baseValueUpdated, _valueUpdated, _capUpdated,_minimumUpdated;

        #endregion
        
        
        #region Instance Variables /////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This list is used to hold the modifiers that are currently applied to this stat.
        /// </summary>
        private List<StatModifier> _modifiers = new List<StatModifier>();
        
        #endregion
        
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This property is used to get the stat's current base value.  This is the stat value before the
        /// modifiers are applied. This property can be used to set the base value only when on the server.
        /// </summary>
        public int BaseValue {
            get { return _baseValue; }
            set {
                if(!IsServer()) return;
                if(value == _baseValue) return;
                _baseValue = value;
                _baseValueUpdated = true;
                CalculateValue();
                Dirty();
            }
        }

        /// <summary>
        /// This property is used to get the stat's value.  This is the stat value after the modifiers are
        /// applied. This property can be used to set the value only when on the server.
        /// </summary>
        public int Value {
            get { return _value; }
            protected set {
                if(!IsServer()) return;
                if(value == _value) return;
                _value = value;
                _valueUpdated = true;
                Dirty();
            }
        }

        /// <summary>
        /// This property is used to get the stat's current cap.  This property can be used to set the cap only
        /// when on the server.  If this value is negative no cap will be applied.
        /// </summary>
        public int Cap {
            get => _cap;
            set {
                if(!IsServer()) return;
                if(value == _cap) return;
                _cap = value;
                _capUpdated = true;
                CalculateValue();
                Dirty();
            }
        }

        /// <summary>
        /// This property is used to get the stat's current minimum value.  This property can be used to set the 
        /// stat's minimum value only when on the server.
        /// </summary>
        public int Minimum {
            get => _minimumValue;
            set {
                if(!IsServer()) return;
                if(value == _minimumValue) return;
                _minimumValue = value;
                _minimumUpdated = true;
                CalculateValue();
                Dirty();
            }
        }

        #endregion
        
        
        #region Modifier Methods ///////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to add a modifier to the stat.  This method can only be called on the server.
        /// </summary>
        /// <param name="modifier">The modifier that you want to add.</param>
        public void AddModifier(StatModifier modifier) {
            if(!IsServer()) return;
            //only allow a single override modifier
            if(modifier.Operation == ModifierOperation.Override) {
                for(var i =0; i < _modifiers.Count;i++)
                    if(_modifiers[i].Operation == ModifierOperation.Additive) {
                        _modifiers[i] = modifier;
                        CalculateValue();
                    }
            }
            _modifiers.Add(modifier);
            CalculateValue();
        }

        /// <summary>
        /// This method is used to remove modifiers with the given source.  This method can only be called on
        /// the server.
        /// </summary>
        /// <param name="source">The source of the modifier.</param>
        public void RemoveModifierFromSource(Object source) {
            if(!IsServer()) return;
            _modifiers = _modifiers.Where(modifier => modifier.HasSource(source)).ToList();
            CalculateValue();
        }

        /// <summary>
        /// This method is used to remove modifiers with the given source.  This method can only be called on
        /// the server.
        /// </summary>
        /// <param name="sourceId">The id of the source of the modifier.</param>
        public void RemoveModifierFromSource(int sourceId) {
            if(!IsServer()) return;
            _modifiers = _modifiers.Where(modifier => modifier.HasSource(sourceId)).ToList();
            CalculateValue();
        }

        /// <summary>
        /// This method is used to clear all of the stat's modifiers.  This method can only be called on the server.
        /// </summary>
        public void ClearAllModifiers() {
            _modifiers.Clear();
            CalculateValue();
        }
        
        #endregion
        

        #region Sync Methods ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true) {
            base.WriteDelta(writer, resetSyncTick);
            writer.WriteBoolean(_baseValueUpdated);
            if(_baseValueUpdated) writer.WriteInt32(_baseValue);
            _baseValueUpdated = false;
            writer.WriteBoolean(_valueUpdated);
            if(_valueUpdated) writer.WriteInt32(_value);
            _valueUpdated = false;
            writer.WriteBoolean(_capUpdated);
            if(_capUpdated) writer.WriteInt32(_cap);
            _capUpdated = false;
            writer.WriteBoolean(_minimumUpdated);
            if(_minimumUpdated) writer.WriteInt32(_minimumValue);
            _minimumUpdated = false;
        }

        /// <inheritdoc />
        public override void WriteFull(PooledWriter writer) {
            writer.WriteBoolean(true);
            writer.WriteInt32(_baseValue);
            writer.WriteBoolean(true);
            writer.WriteInt32(_value);
            writer.WriteBoolean(true);
            writer.WriteInt32(_cap);
            writer.WriteBoolean(true);
            writer.WriteInt32(_minimumValue);
        }

        /// <inheritdoc />
        public override void Read(PooledReader reader) {
            if(reader.ReadBoolean()) _baseValue = reader.ReadInt32();
            if(reader.ReadBoolean()) _value = reader.ReadInt32();
            if(reader.ReadBoolean()) _cap = reader.ReadInt32();
            if(reader.ReadBoolean()) _minimumValue = reader.ReadInt32();
        }
        
        /// <inheritdoc />
        public object GetSerializedType() => null;
        
        #endregion

        
        #region Protected Methods //////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method is used to calculate the value based on
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected void CalculateValue() {
            //start with the base value
            var newValue = BaseValue;
            float multiplier = 0;
            var appliedMultiplier = false;
            //sort the modifiers so that they are applied in the correct order.
            _modifiers.Sort(SortModifiers);
            //apply all of the modifiers
            foreach(var modifier in _modifiers) {
                if(modifier.Operation > ModifierOperation.AdditiveMultiplier && !appliedMultiplier) {
                    if(multiplier!=0) newValue = Mathf.RoundToInt(newValue * multiplier);
                    appliedMultiplier = true;
                }
                switch(modifier.Operation) {
                    case ModifierOperation.Additive: newValue += Mathf.RoundToInt(modifier.Amount); break;
                    case ModifierOperation.AdditiveMultiplier: multiplier += modifier.Amount; break;
                    case ModifierOperation.StackableMultiplier: 
                        newValue = Mathf.RoundToInt(newValue * modifier.Amount); break;
                    case ModifierOperation.PostMultiplierAdditive: newValue += Mathf.RoundToInt(modifier.Amount); break;
                    case ModifierOperation.Override: newValue = Mathf.RoundToInt(modifier.Amount); break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }
            //make sure that multipliers are added if they haven't been
            if(!appliedMultiplier&&multiplier!=0) newValue = Mathf.RoundToInt(newValue * multiplier);
            //make sure that the value does not exceed the cap
            if(Cap >= 0) newValue = Mathf.Min(newValue, Cap);
            //make sure that the value is not less than the minimum
            newValue = Mathf.Max(_minimumValue, newValue);
            //set the new value
            Value = newValue;
        }

        /// <summary>
        /// This method is used to sort the modifiers.
        /// </summary>
        /// <param name="a">The first modifier to compare.</param>
        /// <param name="b">The second modifier to compare.</param>
        /// <returns>The result of the comparison.</returns>
        protected static int SortModifiers(StatModifier a, StatModifier b) {
            return a.Operation.CompareTo(b.Operation);
        }
        
        /// <summary>
        /// This method is used to check if running on the server.  If not on the server it will debug a warning to
        /// the console.
        /// </summary>
        /// <returns>True if running on the server.</returns>
        protected bool IsServer(bool throwException = false) {
            if(NetworkManager.IsServer) return true;
            Debug.LogWarning("Values cannot be changed on the client!");
            if(throwException) throw new Exception("Unable to preform action from client!");
            return false;
        }
        
        #endregion
        
    }
    
}