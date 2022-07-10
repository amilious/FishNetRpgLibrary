using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to get a bool representation for the bit at the given index (0-31) of the value.")]
    public class GetBit : ConvertingNodes {
        
        private CalculationId _lastId;
        private bool _lastValue;

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            inputPorts.Add(new PortInfo<int>("bit index"));
            outputPorts.Add(new PortInfo<bool>("bool",GetValue));
        }

        private bool GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            if(!TryGetPortValue(0, id, out int value)) return _lastValue = false;
            if(!TryGetPortValue(1, id, out int index)) return _lastValue = false;
            if(index is < 0 or >= 32) return _lastValue = false;
            return _lastValue = (value >> index & 0x01)==0x01;
        }
        
    }
}