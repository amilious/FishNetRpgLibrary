using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a int value at the given part of your function.")]
    public class TestInt : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<int>("input"));
            outputPorts.Add( new PortInfo<int>("output", GetInput));
        }

        private CalculationId _lastId;
        private int _lastResult;

        private int GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out int value);
            return _lastResult = value;
        }

        protected override string TestValue(CalculationId id) {
            return GetInput(id).ToString(CultureInfo.InvariantCulture);
        }
        
    }
}