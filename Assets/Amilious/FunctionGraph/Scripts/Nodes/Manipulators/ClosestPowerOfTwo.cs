using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to return the closest power of two in relation to the passed value.")]
    public class ClosestPowerOfTwo : ManipulatorNodes {
        
        private CalculationId _lastId;
        private int _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            outputPorts.Add( new PortInfo<int>("result", GetValue));
        }

        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            return _lastValue = Mathf.ClosestPowerOfTwo(value);
        }

    }
}