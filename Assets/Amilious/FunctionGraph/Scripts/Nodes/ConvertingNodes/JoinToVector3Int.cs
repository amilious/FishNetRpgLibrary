using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to join three int values into a Vector3Int.")]
    public class JointToVector3Int : ConvertingNodes {

        private CalculationId _lastId;
        private Vector3Int _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("x"));
            inputPorts.Add(new PortInfo<int>("y"));
            inputPorts.Add(new PortInfo<int>("z"));
            outputPorts.Add(new PortInfo<Vector3Int>("result",GetValue));
        }

        private Vector3Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int x);
            TryGetPortValue(1, id, out int y);
            TryGetPortValue(2, id, out int z);
            return _lastValue = new Vector3Int(x, y, z);

        }
        
    }
}