using System;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get the square root of the given value.")]
    public class SquareRoot : ManipulatorNodes {

        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            outputPorts.Add(new PortInfo<float>("result",GetResult));
        }

        private float GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value);
            return _lastValue = Mathf.Sqrt(value);
        }
        
    }
}