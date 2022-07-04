using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    [FunctionNode("This node is used to represent a float value for negative infinity.")]
    public class NegativeInfinity : InputNodes {
        
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",GetValue));
        }

        private float GetValue(CalculationId arg) => Mathf.NegativeInfinity;

        #if UNITY_EDITOR

        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Negative Infinity  "));
        }

        #endif
    }
}