using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to set a bool representation for the bit at the given index (0-31) of the value.")]
    public class SetBit : ConvertingNodes {
        
        private CalculationId _lastId;
        private int _lastValue;

        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            inputPorts.Add(new PortInfo<int>("bit index"));
            inputPorts.Add(new PortInfo<bool>("bit value"));
            outputPorts.Add(new PortInfo<int>("result",GetValue));
        }

        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            if(!TryGetPortValue(1, id, out int index)) return _lastValue = value;
            if(index is < 0 or >= 32) return _lastValue = value;
            TryGetPortValue(2, id, out bool bit);
            return _lastValue = bit ? value | (1 << index) : value & ~(1 << index);
        }
        
    }
}