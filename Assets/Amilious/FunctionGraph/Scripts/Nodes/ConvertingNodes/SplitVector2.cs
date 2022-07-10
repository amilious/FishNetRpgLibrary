using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to split a Vector2 into two floats.")]
    public class SplitVector2 : ConvertingNodes {

        private CalculationId _lastId;
        private Vector2 _lastValue;
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector2>("Vector2"));
            outputPorts.Add(new PortInfo<float>("x",GetXValue));
            outputPorts.Add(new PortInfo<float>("y",GetYValue));
        }

        private Vector2 GetVector3(CalculationId id) {
            if(_lastId == id) return _lastValue;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }

        private float GetYValue(CalculationId id) => GetVector3(id).y;

        private float GetXValue(CalculationId id) => GetVector3(id).x;
        
    }
}