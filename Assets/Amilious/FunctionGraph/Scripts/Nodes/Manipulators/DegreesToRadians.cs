using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to convert a value from degrees to radians.")]
    public class DegreesToRadians : ManipulatorNodes {
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("degrees",true));
            outputPorts.Add(new PortInfo<float>("radians",GetResult));
        }

        private CalculationId _lastId;
        private float _lastValue;
        
        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue; 
            _lastId = id;
            TryGetPortValue(0, id, out float degrees);
            return _lastValue = degrees * Mathf.Deg2Rad;
        }
        
    }
}