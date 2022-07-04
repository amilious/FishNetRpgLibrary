using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent the maximum int value.")]
    public class IntMax : InputNodes {
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<int>("",GetValue));
        }

        private int GetValue(CalculationId arg) => int.MaxValue;

        #if UNITY_EDITOR
        
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Max Int  "));
        }

        #endif
    }
}