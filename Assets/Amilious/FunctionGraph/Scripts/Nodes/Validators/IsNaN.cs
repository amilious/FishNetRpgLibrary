using System.Collections.Generic;

namespace Amilious.FunctionGraph.Nodes.Validators {
    public class IsNaN : ValidatorNodes {

        private CalculationId _lastId;
        private bool _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            outputPorts.Add( new PortInfo<bool>("result",GetResult));
        }

        private bool GetResult(CalculationId id) {
            if(_lastId == id) return _lastValue;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }
    }
}