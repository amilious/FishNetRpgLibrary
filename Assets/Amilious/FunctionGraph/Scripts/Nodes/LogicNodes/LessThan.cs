using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    [FunctionNode("This node is used to check if the value is less than the test.")]
    public class LessThan : LogicNodes {
        
        private CalculationId _lastId;
        private bool _lastResult;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("test"));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }
        
        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastResult;
            _lastId = id;
            if(!TryGetPortValue<float>(0, id, out var value)) return _lastResult = false;
            if(!TryGetPortValue<float>(1, id, out var test)) return _lastResult = true;
            return _lastResult = value < test;
        }
        
    }
}