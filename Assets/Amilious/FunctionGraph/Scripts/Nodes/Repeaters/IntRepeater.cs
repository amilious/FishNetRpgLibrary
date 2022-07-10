using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    [FunctionNode("This node is used to to give the int input value as an output.")]
    public class IntRepeater : RepeaterNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>(""));
            outputPorts.Add(new PortInfo<int>("",GetValue));
        }

        private CalculationId _lastId;
        private int _lastValue;
        
        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}