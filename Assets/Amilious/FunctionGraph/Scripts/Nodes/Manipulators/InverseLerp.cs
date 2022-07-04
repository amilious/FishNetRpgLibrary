using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get where a value lies between two values as a percent.")]
    public class InverseLerp : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("min value"));
            inputPorts.Add(new PortInfo<float>("max Value"));
            inputPorts.Add(new PortInfo<float>("value"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float a);
            TryGetPortValue(0, id, out float b);
            if(b > a) { (a, b) = (b, a); }
            TryGetPortValue(0, id, out float t);
            return _lastValue = Mathf.InverseLerp(a,b,t);
        }

    }
}