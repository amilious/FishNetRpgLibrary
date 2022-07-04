using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    [FunctionNode("This node is used to check if the value is greater than or equal to the test.")]
    public class GreaterThanOrEqual : LogicNodes {
        
        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("test"));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            if(!TryGetPortValue<float>(0, id, out var value)) return _lastValue = false;
            if(!TryGetPortValue<float>(1, id, out var test)) return _lastValue = true;
            return _lastValue = value >= test;
        }
    
    }
}