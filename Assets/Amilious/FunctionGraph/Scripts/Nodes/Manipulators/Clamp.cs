using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to clamp the given input between the given minimum and maximum inclusively.")]
    public class Clamp : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value"));
            inputPorts.Add(new PortInfo<float>("minimum"));
            inputPorts.Add(new PortInfo<float>("maximum"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value);
            TryGetPortValue(0, id, out float min);
            TryGetPortValue(0, id, out float max);
            return _lastValue = Mathf.Clamp(value, min, max);
        }

    }
}