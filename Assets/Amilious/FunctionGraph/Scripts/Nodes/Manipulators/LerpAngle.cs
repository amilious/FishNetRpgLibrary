using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to linearly interpolate between value 1 and value 2 by percent making sure the values wrap around 360 degrees.")]
    public class LerpAngle : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value 1"));
            inputPorts.Add(new PortInfo<float>("value 2"));
            inputPorts.Add(new PortInfo<float>("percent"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float a);
            TryGetPortValue(0, id, out float b);
            if(b > a) { (a, b) = (b, a); }
            TryGetPortValue(0, id, out float t);
            return _lastValue = Mathf.LerpAngle(a,b,t);
        }

    }
}