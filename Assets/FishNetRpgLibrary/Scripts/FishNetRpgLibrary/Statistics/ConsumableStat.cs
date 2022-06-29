using FishNet.Serializing;
using UnityEngine;

namespace FishNetRpgLibrary.Statistics {
    
    /// <summary>
    /// This class is used to represent a networked consumable stat.
    /// </summary>
    public class ConsumableStat : Stat {

        
        #region Private Variables //////////////////////////////////////////////////////////////////////////////////////
        
        private float _currentValue;
        private bool _currentValueUpdated;
        
        #endregion
        
        
        #region Properties /////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override bool IsConsumable => true;
        
        /// <inheritdoc />
        public override int Value {
            get => base.Value;
            protected set {
                base.Value = value;
                Percentage = CurrentValue / Value;
            }
        }

        /// <summary>
        /// This property contains the current value of the stat.
        /// </summary>
        public float CurrentValue {
            get => _currentValue;
            protected set {
                if(!IsServer()) return;
                if(value == _currentValue) return;
                _currentValue = value;
                _currentValueUpdated = true;
                Dirty();
                Percentage = CurrentValue / Value;
            }
        }
        
        /// <summary>
        /// This property contains the current percentage of the current value compared to the value.
        /// </summary>
        public float Percentage { get; protected set; }

        /// <summary>
        /// If false the Restore and Consume methods will be disabled, otherwise they will be enabled.
        /// </summary>
        public bool Active { get; set; } = true;
        
        #endregion
        
        
        #region Current Value Modifier Methods /////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This method is used to reduce the current value by the given amount.  This method should only
        /// be called on the server.
        /// </summary>
        /// <param name="amount">The amount that you want to reduce the stat's current value by. </param>
        public virtual void ConsumeCurrentValue(float amount) {
            if(!IsServer()||!Active) return;
            CurrentValue = Mathf.Max(0, CurrentValue - amount);
        }

        /// <summary>
        /// This method is used to reduce the current value by the given percentage of the max value.
        /// This method should only be called on the server.
        /// </summary>
        /// <param name="percentage">The percentage that you want to reduce the stat's current value by. </param>
        public virtual void ConsumePercentage(float percentage) {
            if(!IsServer()||!Active) return;
            var consume = Value * percentage;
            CurrentValue = Mathf.Max(0, CurrentValue - consume);
        }

        /// <summary>
        /// This method is used to reduce the current value by give percentage of the remaining value.
        /// This method should only be called on the server.
        /// </summary>
        /// <param name="percentage">The percentage that you want to reduce the stat's current value by. </param>
        public virtual void ConsumePercentageRemaining(float percentage) {
            if(!IsServer()||!Active) return;
            var consume = CurrentValue * percentage;
            CurrentValue = Mathf.Max(0, CurrentValue - consume);
        }

        /// <summary>
        /// The method is used to restore the current value by the given amount.
        /// This method should only be called on the server.
        /// </summary>
        /// <param name="amount">The amount that you want to restore to the current value.</param>
        public virtual void RestoreCurrentValue(float amount) {
            if(!IsServer()||!Active) return;
            CurrentValue = Mathf.Min(Value, CurrentValue + amount);
        }

        /// <summary>
        /// This method is used to restore the current value by the given percentage of the max value.
        /// This method should only be called on the server.
        /// </summary>
        /// <param name="percentage">The percentage that you want to restore the stat's current value by. </param>
        public virtual void RestorePercentage(float percentage) {
            if(!IsServer()||!Active) return;
            //calculate base value to add
            var add = Value * percentage;
            CurrentValue = Mathf.Min(Value, CurrentValue + add);
        }

        /// <summary>
        /// This method is used to restore the current value by give percentage of the remaining value.
        /// This method should only be called on the server.
        /// </summary>
        /// <param name="percentage">The percentage that you want to restore the stat's current value by. </param>
        public virtual void RestorePercentageRemaining(float percentage) {
            if(!IsServer()||!Active) return;
            //calculate base value to add
            var add = (Value-CurrentValue) * percentage;
            CurrentValue = Mathf.Min(Value, CurrentValue + add);
        }

        /// <summary>
        /// This method is used to restore the current value to the full value.
        /// This should only be called on the server.
        /// </summary>
        public virtual void RestoreFull() {
            if(!IsServer()||!Active) return;
            CurrentValue = Value;
        }

        /// <summary>
        /// This method is used to set the current value to the given percentage of the value.
        /// This should only be called on the server.
        /// </summary>
        /// <param name="percentage">The percentage that you want to restore the stat's current value by. </param>
        public virtual void SetPercentage(float percentage) {
            if(!IsServer()||!Active) return;
            CurrentValue = Mathf.Clamp(Value * percentage, 0, Value);
        }

        /// <summary>
        /// This method is used to set the current value to the given value.
        /// </summary>
        /// <param name="value">The value that you want to set the current value to.</param>
        public virtual void SetCurrentValue(float value) {
            if(!IsServer()||!Active) return;
            CurrentValue = Mathf.Clamp(value, 0, Value);
        }

        #endregion
        
        
        #region Sync Methods ///////////////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        public override void Read(PooledReader reader) {
            base.Read(reader);
            if(reader.ReadBoolean()) _currentValue = reader.ReadSingle();
            //we calculate the percentage here so it is available on the client
            Percentage = CurrentValue / Value;
        }

        /// <inheritdoc />
        public override void WriteDelta(PooledWriter writer, bool resetSyncTick = true) {
            base.WriteDelta(writer, resetSyncTick);
            writer.WriteBoolean(_currentValueUpdated);
            if(_currentValueUpdated) writer.WriteSingle(_currentValue);
            _currentValueUpdated = false;
        }

        /// <inheritdoc />
        public override void WriteFull(PooledWriter writer) {
            base.WriteFull(writer);
            writer.WriteBoolean(true);
            writer.WriteSingle(_currentValue);
        }

        #endregion
        
    }
    
}