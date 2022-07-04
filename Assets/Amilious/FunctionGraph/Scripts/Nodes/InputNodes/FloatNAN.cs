using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent the float NaN value.")]
    public class FloatNAN : InputNodes {
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",GetValue));
        }

        private float GetValue(CalculationId arg) => float.NaN;

        #if UNITY_EDITOR
        
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new Label(" Float NaN  "));
        }

        #endif
    }
}