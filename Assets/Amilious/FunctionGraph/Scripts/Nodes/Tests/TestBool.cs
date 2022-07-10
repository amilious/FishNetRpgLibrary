using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a bool value at the given part of your function.")]
    public class TestBool : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<bool>("input"));
            outputPorts.Add( new PortInfo<bool>("output", GetInput));
        }

        private CalculationId _lastId;
        private bool _lastResult;

        private bool GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out bool value);
            _lastResult = value;
            #if UNITY_EDITOR
            SetLabel(id,_lastResult? "true" : "false");
            #endif
            return _lastResult;
        }

        protected override void TestValue(CalculationId id) => GetInput(id);
    }
}