using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to convert a value from radians to degrees.")]
    public class RadiansToDegrees : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("radians",true));
            outputPorts.Add(new PortInfo<float>("degrees",GetResult));
        }
        
        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue; 
            _lastId = id;
            TryGetPortValue(0, id, out float radians);
            return _lastValue = radians * Mathf.Rad2Deg;
        }
        
    }
}