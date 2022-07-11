using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Amilious.FunctionGraph.Attributes;

namespace Amilious.FunctionGraph.Nodes.InputNodes {
    
    /// <summary>
    /// This node is used to represent a tiny value.
    /// </summary>
    [FunctionNode("This node is used to represent a tiny value.")]
    public class Epsilon : InputNodes {
        
        #region Non-Editor Only Methods ////////////////////////////////////////////////////////////////////////////////

        /// <inheritdoc />
        protected override void SetUpPorts(List<IPortInfo> inputPorts, List<IPortInfo> outputPorts) {
            outputPorts.Add(new PortInfo<float>("",_=>Mathf.Epsilon));
        }
        
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Editor Only Methods ////////////////////////////////////////////////////////////////////////////////////
        #if UNITY_EDITOR

        /// <inheritdoc />
        public override void ModifyNodeView(UnityEditor.Experimental.GraphView.Node nodeView) {
            base.ModifyNodeView(nodeView);
            nodeView.inputContainer.Add(new  Label(" Epsilon  "));
        }

        #endif
        #endregion /////////////////////////////////////////////////////////////////////////////////////////////////////
                   
    }
}