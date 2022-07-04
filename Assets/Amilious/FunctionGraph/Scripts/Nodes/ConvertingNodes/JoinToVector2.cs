using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to joint two floats together to create a Vector2")]
    public class JoinToVector2 : ConvertingNodes {
        
        private CalculationId _lastId;
        private Vector2 _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add( new PortInfo<float>("x"));
            inputPorts.Add( new PortInfo<float>("y"));
            outputPorts.Add(new PortInfo<Vector2>("Vector2",GetVector2));
        }

        private Vector2 GetVector2(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float x);
            TryGetPortValue(1, id, out float y);
            return _lastValue = new Vector2(x, y);
        }
        
    }
}