using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    /// <summary>
    /// This node is used to set a bool representation for the bit at the given index (0-31) of the value.
    /// </summary>
    [FunctionNode("This node is used to set a bool representation for the bit at the given index (0-31) of the value.")]
    public class SetBit : ConvertingNodes {
        
        #region Non-Serialized Feilds //////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// This field is used to store the last calculation id.
        /// </summary>
        private CalculationId _lastId;
        
        /// <summary>
        /// This field is used to store the last calculated value.
        /// </summary>
        private int _lastValue;
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            inputPorts.Add(new PortInfo<int>("bit index"));
            inputPorts.Add(new PortInfo<bool>("bit value"));
            outputPorts.Add(new PortInfo<int>("result",GetValue));
        }

        /// <summary>
        /// This method is used to get the value of this nodes first output port.
        /// </summary>
        /// <param name="id">The calculation id.</param>
        /// <returns>The value.</returns>
        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            if(!TryGetPortValue(1, id, out int index)) return _lastValue = value;
            if(index is < 0 or >= 32) return _lastValue = value;
            TryGetPortValue(2, id, out bool bit);
            return _lastValue = bit ? value | (1 << index) : value & ~(1 << index);
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }
}