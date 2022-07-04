using System.Linq;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.LogicNodes {
    
    [FunctionNode("This node is used to check if only one value is false.")]
    public class Nand : LogicNodes {
        
        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>("values",true));
            outputPorts.Add(new PortInfo<bool>("result", GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValues<bool>(0, id, out var values);
            if(values == null || values.Count == 0) return _lastValue = true;
            return _lastValue = values.Count(x=>!x)==1;
        }
        
    }
}