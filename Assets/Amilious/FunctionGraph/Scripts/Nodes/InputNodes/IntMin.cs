using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent the minimum int value.")]
    public class IntMin : InputNodes {
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<int>("",GetValue));
        }

        private int GetValue(CalculationId arg) => int.MinValue;

        #if UNITY_EDITOR
        
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Min Int  "));
        }

        #endif
        
    }
}