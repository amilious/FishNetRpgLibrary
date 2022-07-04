using UnityEngine;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.ConvertingNodes {
    
    [FunctionNode("This node is used to split a Vector3 into three floats.")]
    public class SplitVector3 : ConvertingNodes {

        private CalculationId _lastId;
        private Vector3 _lastValue;
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            inputPorts.Add(new PortInfo<Vector3>("Vector3"));
            outputPorts.Add(new PortInfo<float>("x",GetXValue));
            outputPorts.Add(new PortInfo<float>("y",GetYValue));
            outputPorts.Add(new PortInfo<float>("z",GetZValue));
        }

        private Vector3 GetVector3(CalculationId id) {
            if(_lastId == id) return _lastValue;
            TryGetPortValue(0, id, out _lastValue);
            return _lastValue;
        }

        private float GetZValue(CalculationId id) => GetVector3(id).z;

        private float GetYValue(CalculationId id) => GetVector3(id).y;

        private float GetXValue(CalculationId id) => GetVector3(id).x;
        
    }
}