using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to join two int values into a Vector2Int.")]
    public class JointToVector2Int : ConvertingNodes {

        private CalculationId _lastId;
        private Vector2Int _lastValue;
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("x"));
            inputPorts.Add(new PortInfo<int>("y"));
            outputPorts.Add(new PortInfo<Vector2Int>("result",GetValue));
        }

        private Vector2Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int x);
            TryGetPortValue(1, id, out int y);
            return _lastValue = new Vector2Int(x, y);
        }
        
    }
}