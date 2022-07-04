using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get a 2D perlin noise value for the given x and y coordinates.")]
    public class PerlinNoise : ManipulatorNodes {
        
        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("x"));
            inputPorts.Add(new PortInfo<float>("y"));
            outputPorts.Add( new PortInfo<float>("result", GetValue));
        }

        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float x);
            TryGetPortValue(1, id, out float y);
            return _lastValue = Mathf.PerlinNoise(x,y);
        }
        
    }
}