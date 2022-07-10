using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a Vector2 value at the given part of your function.")]
    public class TestVector2 : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector2>("input"));
            outputPorts.Add( new PortInfo<Vector2>("output", GetInput));
        }

        private CalculationId _lastId;
        private Vector2 _lastResult;

        private Vector2 GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out Vector2 value);
            _lastResult = value;
            #if UNITY_EDITOR
            SetLabel(id,_lastResult.ToString());
            #endif
            return _lastResult;
        }

        protected override void TestValue(CalculationId id) => GetInput(id);

    }
}