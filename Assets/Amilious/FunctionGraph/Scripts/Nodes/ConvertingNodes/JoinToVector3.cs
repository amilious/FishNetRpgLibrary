using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to joint three floats together to create a Vector3")]
    public class JoinToVector3 : ConvertingNodes {
        
        private CalculationId _lastId;
        private Vector3 _lastValue;
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add( new PortInfo<float>("x"));
            inputPorts.Add( new PortInfo<float>("y"));
            inputPorts.Add( new PortInfo<float>("z"));
            outputPorts.Add(new PortInfo<Vector3>("Vector3",GetVector3));
        }

        private Vector3 GetVector3(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float x);
            TryGetPortValue(1, id, out float y);
            TryGetPortValue(2, id, out float z);
            return _lastValue = new Vector3(x, y, z);
        }
    }
}