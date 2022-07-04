using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used for converting a integer to a float.")]
    public class IntToFloat : ConvertingNodes {
        
        private CalculationId _lastAction;
        private float _lastResult;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("int"));
            outputPorts.Add(new PortInfo<float>("float", GetValue));
        }

        private float GetValue(CalculationId calculationId) {
            if(calculationId == _lastAction) return _lastResult;
            _lastAction = calculationId;
            TryGetPortValue(0, calculationId, out int value);
            _lastResult = value;
            return _lastResult;
        }

    }
}