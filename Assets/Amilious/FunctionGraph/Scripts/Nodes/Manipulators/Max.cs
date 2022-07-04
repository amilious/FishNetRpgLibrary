using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to get the maximum value in a collection of values.")]
    public class Max : ManipulatorNodes {

        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("inputs",true));
            outputPorts.Add(new PortInfo<float>("max",GetResult));
        }

        private float GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue; 
            _lastId = id;
            TryGetPortValues<float>(0, id, out var values);
            if(values == null || values.Count == 0)  return _lastValue = 0f;
            return _lastValue = Mathf.Max(values.ToArray());
        }
        
    }
}