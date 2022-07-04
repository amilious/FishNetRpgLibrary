using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Validators {
    
    [FunctionNode("This node is used to check if a value is a power of two.")]
    public class IsPowerOfTwo : ValidatorNodes {

        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("value"));
            outputPorts.Add(new PortInfo<bool>("result",GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            return _lastValue = Mathf.IsPowerOfTwo(value);
        }
        
    }
}