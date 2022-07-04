using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get the arc tangent of the provided angle in radians.")]
    public class ArcTangent : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("radians",true));
            outputPorts.Add(new PortInfo<float>("value",GetResult));
        }
        
        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue; 
            _lastId = id;
            TryGetPortValue(0, id, out float radians);
            return _lastValue = Mathf.Atan(radians);
        }
        
    }
}