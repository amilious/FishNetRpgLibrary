using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a float value at the given part of your function.")]
    public class TestFloat : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<float>("input"));
            outputPorts.Add( new PortInfo<float>("output", GetInput));
        }

        private CalculationId _lastId;
        private float _lastResult;

        private float GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out float value);
            return _lastResult = value;
        }

        protected override string TestValue(CalculationId id) {
            return GetInput(id).ToString(CultureInfo.InvariantCulture);
        }
        
    }
}