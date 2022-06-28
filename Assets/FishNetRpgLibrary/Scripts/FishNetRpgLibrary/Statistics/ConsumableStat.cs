using FishNet.Serializing;
using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    public class ConsumableStat : Stat {

        private float _currentValue;

        private bool _currentValueUpdated;

        public float CurrentValue {
            get => _currentValue;
            protected set {
                if(!IsServer()) return;
                if(value == _currentValue) return;
                _currentValue = value;
                _currentValueUpdated = true;
                Dirty();
            }
        }
        
        public override void Read(PooledReader reader) {
            base.Read(reader);
            if(reader.ReadBoolean()) _currentValue = reader.ReadSingle();
        }

        public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true) {
            base.WriteDelta(writer, resetSyncTick);
            writer.WriteBoolean(_currentValueUpdated);
            if(_currentValueUpdated) writer.WriteSingle(_currentValue);
            _currentValueUpdated = false;
        }

        public override void WriteFull(PooledWriter writer) {
            base.WriteFull(writer);
            writer.WriteBoolean(true);
            writer.WriteSingle(_currentValue);
        }

        public virtual float ConsumeCurrentValue(float value) {
            if(!IsServer()) return _currentValue;
            //TODO run value through standard stats to see if it needs to be modified first
            _currentValue = Mathf.Max(0, CurrentValue - value);
            return CurrentValue;
        }

        public virtual float ConsumePercentage(float percentage) {
            if(!IsServer()) return _currentValue;
            var consumeBase = Value * percentage;
            //TODO run value through standard stats to see if it needs to be modified first
            _currentValue = Mathf.Max(0, CurrentValue - consumeBase);
            return CurrentValue;
        }

        public virtual float RestoreCurrentValue(float value) {
            if(!IsServer()) return _currentValue;
            //TODO run value through standard stats to see if it needs to be modified first
            _currentValue = Mathf.Min(Value, CurrentValue + value);
            return CurrentValue;
        }

        public virtual float RestorePercentage(float percentage) {
            if(!IsServer()) return _currentValue;
            //calculate base value to add
            var addBase = Value * percentage;
            //TODO run addBase through standard stats to see if it needs to be modified first.
            _currentValue = Mathf.Min(Value, CurrentValue + addBase);
            return CurrentValue;
        }

        public virtual float SetPercentage(float percentage) {
            if(!IsServer()) return _currentValue;
            _currentValue = Mathf.Clamp(Value * percentage, 0, Value);
            return _currentValue;
        }

        public virtual float SetCurrentValue(float value) {
            if(!IsServer()) return _currentValue;
            _currentValue = Mathf.Clamp(value, 0, Value);
            return CurrentValue;
        }
        
    }
    
}