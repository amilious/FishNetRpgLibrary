using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get the angle in radians who's tangent is y/x.")]
    public class ArcTangent2 : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("y"));
            inputPorts.Add(new PortInfo<float>("x"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float y);
            TryGetPortValue(1, id, out float x);
            return _lastValue = Mathf.Atan2(y,x);
        }
        
    }
}