using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    [FunctionNode("This node is used to to give the Vector2 input value as an output.")]
    public class Vector2Repeater : RepeaterNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector2>(""));
            outputPorts.Add(new PortInfo<Vector2>("",GetValue));
        }

        private CalculationId _lastId;
        private Vector2 _lastValue;
        
        private Vector2 GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}