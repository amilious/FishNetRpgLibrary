using System;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to divide the input by the divisor.")]
    public class Division : ManipulatorNodes {

        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            inputPorts.Add(new PortInfo<float>("divisor"));
            outputPorts.Add(new PortInfo<float>("result",Divide));
        }
        
        private float Divide(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float first);
            if(!TryGetPortValue(1, id, out float second)) return _lastValue = first;
            return _lastValue = first / second;
        }
        
    }
}