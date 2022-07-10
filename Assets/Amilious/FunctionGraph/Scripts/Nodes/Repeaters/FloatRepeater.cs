using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Repeaters {
    
    [FunctionNode("This node is used to to give the float input value as an output.")]
    public class FloatRepeater : RepeaterNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>(""));
            outputPorts.Add(new PortInfo<float>("",GetValue));
        }

        private CalculationId _lastId;
        private float _lastValue;
        
        private float GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}