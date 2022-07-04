using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Manipulators {
    
    [FunctionNode("This node is used to multiply a collection of values.")]
    public class Multiply : ManipulatorNodes {

        private CalculationId _lastId;
        private float _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("inputs",true));
            outputPorts.Add(new PortInfo<float>("result",GetValue));
        }
        
        private float GetValue(CalculationId id) {
            if(id == _lastId) return _lastValue;
            _lastId = id;
            if(!TryGetPortValues<float>(0, id, out var values)) return _lastValue = 0;
            for(var i = 0; i < values.Count; i++) 
                if(i == 0) _lastValue = values[0];
                else _lastValue *= values[i];
            return _lastValue;
        }
        
    }
}