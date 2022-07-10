using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to split a Vector3Int into three ints.")]
    public class SplitVector3Int : ConvertingNodes {

        private CalculationId _lastId;
        private Vector3Int _lastValue;
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3Int>("value"));
            outputPorts.Add( new PortInfo<int>("x",GetX));
            outputPorts.Add( new PortInfo<int>("y",GetY));
            outputPorts.Add( new PortInfo<int>("z",GetZ));
        }

        private Vector3Int GetValue(CalculationId id) {
            if(_lastId == id) return _lastValue;
            _lastId = id;
            TryGetPortValue(0, id, out Vector3Int value);
            return _lastValue = value;
        }

        private int GetZ(CalculationId id) => GetValue(id).z;

        private int GetY(CalculationId id) => GetValue(id).y;

        private int GetX(CalculationId id) => GetValue(id).x;
        
    }
}