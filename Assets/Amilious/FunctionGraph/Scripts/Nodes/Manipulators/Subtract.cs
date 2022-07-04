using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to subtract the second value from the first value.")]
    public class Subtract: ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("subtract"));
            outputPorts.Add(new PortInfo<float>("result",GetValue));
        }
        
        private float GetValue(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float first);
            TryGetPortValue(1, id, out float second);
            return _lastValue = first - second;
        }
        
    }
}