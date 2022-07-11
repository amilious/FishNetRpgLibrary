using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    /// <summary>
    /// This node is used to represent a float value for negative infinity.
    /// </summary>
    [FunctionNode("This node is used to represent a float value for negative infinity.")]
    public class NegativeInfinity : InputNodes {
        
        #region Non-Editor Only Methods ////////////////////////////////////////////////////////////////////////////////
        
        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",_=>Mathf.NegativeInfinity));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Negative Infinity  "));
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

    }
}