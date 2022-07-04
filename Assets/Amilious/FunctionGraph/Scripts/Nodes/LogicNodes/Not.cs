using System.Linq;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    [FunctionNode("This node is used to invert a bool.")]
    public class Not : LogicNodes {
        
        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>("value"));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue<bool>(0, id, out var value);
            return _lastValue = !value;
        }
        
    }
}