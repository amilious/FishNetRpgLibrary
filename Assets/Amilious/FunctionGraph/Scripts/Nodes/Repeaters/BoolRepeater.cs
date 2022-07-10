using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    [FunctionNode("This node is used to to give the bool input value as an output.")]
    public class BoolRepeater : RepeaterNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>(""));
            outputPorts.Add(new PortInfo<bool>("",GetValue));
        }

        private CalculationId _lastId;
        private bool _lastValue;
        
        private bool GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}