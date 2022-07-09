using System.Globalization;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.Tests {
    
    [FunctionNode("This node is used to test a Vector3Int value at the given part of your function.")]
    public class TestVector3Int : TestNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3Int>("input"));
            outputPorts.Add( new PortInfo<Vector3Int>("output", GetInput));
        }

        private CalculationId _lastId;
        private Vector3Int _lastResult;

        private Vector3Int GetInput(CalculationId id) {
            if(_lastId == id) return _lastResult;
            _lastId = id;
            TryGetPortValue(0, id, out Vector3Int value);
            return _lastResult = value;
        }

        protected override string TestValue(CalculationId id) {
            return GetInput(id).ToString();
        }
        
    }
}