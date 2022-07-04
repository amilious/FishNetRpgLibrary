using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Validators {
    
    [FunctionNode("This node is used to check if a value is positive.")]
    public class IsPositive : ValidatorNodes {

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
            return _lastValue = value>-1;
        }
        
    }
}