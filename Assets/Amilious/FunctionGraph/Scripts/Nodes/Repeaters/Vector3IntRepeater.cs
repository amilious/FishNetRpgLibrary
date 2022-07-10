using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    [FunctionNode("This node is used to to give the Vector3Int input value as an output.")]
    public class Vector3IntRepeater : RepeaterNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3Int>(""));
            outputPorts.Add(new PortInfo<Vector3Int>("",GetValue));
        }

        private CalculationId _lastId;
        private Vector3Int _lastValue;
        
        private Vector3Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}