using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    [FunctionNode("This node is used to check if the given values are approximately equal.")]
    public class Approximately : LogicNodes {
        
        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("value 1"));
            inputPorts.Add(new PortInfo<float>("value 2"));
            outputPorts.Add(new PortInfo<bool>("result",GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out float value1);
            TryGetPortValue(1, id, out float value2);
            return _lastValue = Mathf.Approximately(value1, value2);
        }
    }
}