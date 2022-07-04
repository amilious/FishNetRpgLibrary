using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent a tiny value.")]
    public class Epsilon : InputNodes {
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",GetValue));
        }

        private float GetValue(CalculationId arg) => Mathf.Epsilon;

        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Epsilon  "));
        }

        #endif
    }
}