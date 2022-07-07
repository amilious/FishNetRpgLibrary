using System;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.Loops {
    
    [FunctionNode("",true)]
    public class StartLoop : LoopNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("value",GetValue));
            outputPorts.Add(new PortInfo<int>("index",GetIndex));
        }

        private int GetIndex(CalculationId arg) {
            throw new NotImplementedException();
        }

        private float GetValue(CalculationId arg) {
            throw new NotImplementedException();
        }
    }
}