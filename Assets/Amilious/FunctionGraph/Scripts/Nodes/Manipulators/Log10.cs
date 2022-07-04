using System;
using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get the base 10 logarithm of the given value.")]
    public class Log10 : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value);
            return _lastValue = Mathf.Log10(value);
        }
        
    }
}