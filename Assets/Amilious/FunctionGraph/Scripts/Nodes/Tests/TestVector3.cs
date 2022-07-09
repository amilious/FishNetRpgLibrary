using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a Vector3 value at the given part of your function.")]
    public class TestVector3 : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3>("input"));
            outputPorts.Add( new PortInfo<Vector3>("output", GetInput));
        }

        private CalculationId _lastId;
        private Vector3 _lastResult;

        private Vector3 GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out Vector3 value);
            return _lastResult = value;
        }

        protected override string TestValue(CalculationId id) {
            return GetInput(id).ToString();
        }
        
    }
}