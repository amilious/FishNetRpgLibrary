using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to join bool representations of bits into an integer.")]
    public class JoinToInt : ConvertingNodes {

        private CalculationId _lastId;
        private int _lastValue;
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<int>("result", GetValue));
            for(var i=0;i<32;i++) inputPorts.Add(new PortInfo<bool>(i.ToString()));
        }

        private int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            var val = 0;
            for(var i = 0; i < 31; i++) {
                TryGetPortValue(i, id, out bool bit);
                val = bit ? val | (1 << i) : val & ~(1 << i);
            }
            return _lastValue = val;
        }
    }
    
    
}